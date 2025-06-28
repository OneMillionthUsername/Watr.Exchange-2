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
using System.Runtime.CompilerServices;
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
            catch (Exception ex)
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
        where TCommand : DeleteVertex
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
        where TCommand : HardDeleteVertex
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
    public class CreateSingleEdgeCommandHandler<TCommand, TEdge, TFrom, TTo> : IRequestHandler<TCommand, string>
        where TFrom : IVertex
        where TTo : IVertex
        where TEdge : ISingleEdge<TFrom, TTo>, new()
        where TCommand : CreateSingleEdge<TEdge, TFrom, TTo>
    {
        protected IGremlinQuerySource G { get; }
        protected ILogger Logger { get; }
        public CreateSingleEdgeCommandHandler(IGremlinQuerySource q, ILogger<TCommand> logger)
        {
            G = q;
            Logger = logger;
        }
        public virtual async Task<string> Handle(TCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var e = request.Edge;

                var ed = await G.V<TFrom>(e.From.Id).AddE(e).To(__ =>
                    __.V<TTo>(e?.To?.Id ?? throw new InvalidDataException())).FirstAsync(cancellationToken);
                return ed.Id;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
    public class CreateMultiEdgeCommandHandler<TCommand, TEdgeValue, TEdge, TFrom, TTo> : IStreamRequestHandler<TCommand, string>
        where TFrom : IVertex
        where TTo : IVertex
        where TEdgeValue: IEdgeValue<TFrom, TTo>
        where TEdge : IMultiEdge<TEdgeValue, TFrom, TTo>, new()
        where TCommand : CreateMultiEdge<TEdgeValue, TEdge, TFrom, TTo>
    {
        protected IGremlinQuerySource G { get; }
        protected ILogger Logger { get; }
        public CreateMultiEdgeCommandHandler(IGremlinQuerySource q, ILogger<TCommand> logger)
        {
            G = q;
            Logger = logger;
        }

        public virtual async IAsyncEnumerable<string> Handle(TCommand request, [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            var e = request.Edge;
            foreach (var to in e)
            {
                var ed = await G.V<TFrom>(to.From.Id).AddE(to).To(__ =>
                    __.V<TTo>(to.To?.Id ?? throw new InvalidDataException())).FirstAsync(cancellationToken);
                yield return ed.Id;
            }
        }
    }
    public class UpdateSingleEdgeCommandHandler<TCommand, TEdge, TFrom, TTo> : IRequestHandler<TCommand, string>
        where TFrom : IVertex
        where TTo : IVertex
        where TEdge : ISingleEdge<TFrom, TTo>, new()
        where TCommand : UpdateSingleEdge<TEdge, TFrom, TTo>
    {
        protected IGremlinQuerySource G { get; }
        protected ILogger Logger { get; }
        public UpdateSingleEdgeCommandHandler(IGremlinQuerySource q, ILogger<TCommand> logger)
        {
            G = q;
            Logger = logger;
        }

        public virtual async Task<string> Handle(TCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.Edge.To?.Id))
                    throw new InvalidDataException();
                var to = await G.E<TEdge>(request.Edge.Id).Where(e => e.IsDeleted == false).InV<TTo>().SingleAsync(cancellationToken);
                if (to.Id != request.Edge.To.Id)
                {
                    await G.E<TEdge>(request.Edge.Id).Drop().FirstOrDefaultAsync(cancellationToken);
                    var ed = await G.V<TFrom>(request.Edge.From.Id).AddE(request.Edge).To(__ =>
                        __.V<TTo>(request.Edge.To.Id)).FirstAsync(cancellationToken);
                    return ed.Id;

                }
                else
                {
                    var e = G.E<TEdge>(request.Edge.Id);
                    foreach (var (pi, val) in request.Edge.GetNonIgnoredStringProperties())
                    {
                        e = e.Property(pi.Name, val);
                    }
                    var ed = await e.FirstAsync(cancellationToken);
                    return ed.Id;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
    public class UpdateMultipleEdgeCommandHandler<TCommand, TEdgeValue, TEdge, TFrom, TTo> : IStreamRequestHandler<TCommand, string>
        where TFrom : IVertex
        where TTo : IVertex
        where TEdgeValue : IEdgeValue<TFrom, TTo>
        where TEdge : IMultiEdge<TEdgeValue, TFrom, TTo>, new()
        where TCommand : UpdateMultiEdge<TEdgeValue, TEdge, TFrom, TTo>
    {
        protected IGremlinQuerySource G { get; }
        protected ILogger Logger { get; }
        public UpdateMultipleEdgeCommandHandler(IGremlinQuerySource q, ILogger<TCommand> logger)
        {
            G = q;
            Logger = logger;
        }

        public virtual async IAsyncEnumerable<string> Handle(TCommand request, [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            var e = request.Edge;
            if (!e.Any())
                yield break;
            var fromId = e.First().From.Id;
            var tos = await G.V<TFrom>(fromId).Out<TEdgeValue>().OfType<TTo>().ToArrayAsync(cancellationToken);
            HashSet<string> ids = new HashSet<string>();
            foreach (var to in tos)
                ids.Add(to.Id);
            foreach (var id in ids.Where(i => !e.Any(t => t.Id == i)))
            {
                await G.E<TEdge>(id).Drop().FirstOrDefaultAsync();
            }
            foreach (var to in request.Edge.Where(ed => !ids.Contains(ed.Id)))
            {
                var ed = await G.V<TFrom>(fromId).AddE(to).To(__ =>
                        __.V<TTo>(to.To?.Id ?? throw new InvalidDataException())).FirstAsync(cancellationToken);
                yield return ed.Id;
            }
            foreach (var to in request.Edge.Where(ed => ids.Contains(ed.Id)))
            {
                var ev = G.E<TEdgeValue>(to.Id);
                foreach (var (pi, val) in request.Edge.GetNonIgnoredStringProperties())
                {
                    ev = ev.Property(pi.Name, val);
                }
                var eg = await ev.FirstAsync(cancellationToken);
                yield return eg.Id;
            }
        }
    }
    
    public class DeleteEdgeCommandHandler<TCommand> : IRequestHandler<TCommand, Unit>
        where TCommand : DeleteEdge
    {
        protected IGremlinQuerySource G { get; }
        protected ILogger Logger { get; }
        public DeleteEdgeCommandHandler(IGremlinQuerySource q, ILogger<TCommand> logger)
        {
            G = q;
            Logger = logger;
        }

        public virtual async Task<Unit> Handle(TCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await G.E<Data.Core.Edge>(request.Id).Property(p => p.IsDeleted, true).FirstOrDefaultAsync();
                return Unit.Value;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
    public class HardDeleteEdgeCommandHandler<TCommand> : DeleteEdgeCommandHandler<TCommand>
        where TCommand : HardDeleteEdge
    {
        public HardDeleteEdgeCommandHandler(IGremlinQuerySource q, ILogger<TCommand> logger) : base(q, logger)
        {
        }
        public override async Task<Unit> Handle(TCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await G.E(request.Id).Drop().FirstOrDefaultAsync();
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
