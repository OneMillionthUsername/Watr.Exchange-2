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
    public static class WatrExchangeCommandHandlers { }
    internal static class Extensions
    {
        /// <summary>
        /// Iterate over every public instance string-property on <typeparamref name="T"/>,
        /// yielding only those whose value is non-null, non-empty, and not equal to the sentinel.
        /// </summary>
        public static IEnumerable<(PropertyInfo Prop, object? Value)>
          GetNonIgnoredStringProperties<T>(
            this T obj,
            string sentinel = StringIgnore.Ignore)
        {
            // grab all public instance props of type string
            var strProps = typeof(T)
              .GetProperties(BindingFlags.Public | BindingFlags.Instance)
              .Where(pi =>
                 pi.CanRead
              && pi.CanWrite
              && pi.PropertyType == typeof(string));

            foreach (var pi in strProps)
            {
                var val = (string?)pi.GetValue(obj);

                if (val == sentinel)
                    continue;

                yield return (pi, val);
            }
            var nonStrProps = typeof(T)
              .GetProperties(BindingFlags.Public | BindingFlags.Instance)
              .Where(pi =>
                 pi.CanRead
              && pi.CanWrite
              && pi.PropertyType != typeof(string));
            foreach (var pi in nonStrProps)
            {
                var val = pi.GetValue(obj);

                yield return (pi, val);
            }
        }
    }
    public class CreateCommandHandler<TCommand, TVertex> : IRequestHandler<TCommand, string>
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
        public virtual async Task<string> Handle(TCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var d = await G.AddV(request.Vertex).FirstAsync(cancellationToken);
                return d.Id;
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
                var v = await G.V(request.Vertex.Id).OfType<Vertex>().FirstAsync(cancellationToken);
                request.Vertex.CreateDate = v.CreateDate;
                request.Vertex.CreatedByUserId = v.CreatedByUserId;
                request.Vertex.IsDeleted = v.IsDeleted;
                request.Vertex.UpdateDate = DateTime.UtcNow;
                var upd = G.V(request.Vertex.Id).OfType<TVertex>();
                foreach (var (pi, val) in request.Vertex.GetNonIgnoredStringProperties())
                {
                    upd = upd.Property(pi.Name, val);
                }
                await upd.FirstAsync(cancellationToken);
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
    public class HardDeleteCommandHandler<TCommand> : DeleteCommandHandler<TCommand>
        where TCommand: HardDeleteVertex
    {
        public HardDeleteCommandHandler(IGremlinQuerySource q, ILogger<TCommand> logger) : base(q, logger)
        {
        }

        public override async Task<Unit> Handle(TCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await G.V(request.Id).Drop().FirstAsync(cancellationToken);
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
