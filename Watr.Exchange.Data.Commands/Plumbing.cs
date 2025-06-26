using ExRam.Gremlinq.Core;
using Gremlin.Net.Structure;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Watr.Exchange.Core;
using Watr.Exchange.Data.Commands.Core;
using Watr.Exchange.Data.Core;
using Vertex = Watr.Exchange.Data.Core.Vertex;

namespace Watr.Exchange.Data.Commands
{
    internal static class Extensions
    {
        private static ConcurrentDictionary<string, PropertyInfo[]> PropertyMap = new ConcurrentDictionary<string, PropertyInfo[]>();
        public static void NullifySentinelValues<TVertex>(this TVertex vertex, string sentinel = StringIgnore.Ignore)
            where TVertex: IVertex
        {
            string typeName = typeof(TVertex).FullName!;
            if (!PropertyMap.TryGetValue(typeName, out var props))
            {
                props = typeof(TVertex)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance);
                PropertyMap.TryAdd(typeName, props);
            }

            foreach (var pi in props)
            {
                if (!pi.CanRead || !pi.CanWrite)
                    continue;

                var val = pi.GetValue(vertex);
                if (val == null)
                    continue;

                // only null out reference‐type props or nullable value‐types
                var isNullableValueType =
                    Nullable.GetUnderlyingType(pi.PropertyType) != null;
                if (pi.PropertyType.IsClass || isNullableValueType)
                {
                    // here we do whatever comparison makes sense:
                    // e.g. string equality, numeric equality, etc.
                    if (Equals(val, sentinel))
                    {
                        pi.SetValue(vertex, null);
                    }
                }
            }
        }
    }
    public abstract class CreateCommandHandler<TCommand, TVertex> : IRequestHandler<TCommand, Unit>
        where TVertex : IVertex, IConcrete, new()
        where TCommand : CreateVertex<TVertex>
    {
        protected IGremlinQuerySource G { get; }
        protected ILogger Logger { get; }
        public CreateCommandHandler(IGremlinQuerySource q, ILogger<TCommand> logger)
        {
            G = q;
            Logger = logger;
        }
        public virtual async Task<Unit> Handle(TCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await G.AddV(request.Vertex).FirstAsync(cancellationToken);
                return Unit.Value;
            }
            catch(Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
    public class UpdateCommandHandler<TCommand, TVertex> : IRequestHandler<TCommand, Unit>
        where TVertex : IVertex
        where TCommand : UpdateVertex<TVertex>
    {
        protected IGremlinQuerySource G { get; }
        protected ILogger Logger { get; }
        public UpdateCommandHandler(IGremlinQuerySource q, ILogger<TCommand> logger)
        {
            G = q;
            Logger = logger;
        }
        public virtual async Task<Unit> Handle(TCommand request, CancellationToken cancellationToken)
        {
            try
            {
                request.Vertex.NullifySentinelValues();
                var v = await G.V(request.Vertex.Id).OfType<Vertex>().FirstAsync(cancellationToken);
                request.Vertex.CreateDate = v.CreateDate;
                request.Vertex.CreatedByUserId = v.CreatedByUserId;
                request.Vertex.IsDeleted = v.IsDeleted;
                await G.V(request.Vertex.Id).OfType<TVertex>().Update(request.Vertex).FirstAsync(cancellationToken);
                return Unit.Value;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
    public class DeleteCommandHandler<TCommand> : IRequestHandler<TCommand, Unit>
        where TCommand: DeleteVertex
    {
        protected IGremlinQuerySource G { get; }
        protected ILogger Logger { get; }
        public DeleteCommandHandler(IGremlinQuerySource q, ILogger<TCommand> logger)
        {
            G = q;
            Logger = logger;
        }

        public virtual async Task<Unit> Handle(TCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await G.V(request.Id).OfType<Vertex>().
                    Property(p => p.IsDeleted, true).FirstAsync(cancellationToken);
                return Unit.Value;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}
