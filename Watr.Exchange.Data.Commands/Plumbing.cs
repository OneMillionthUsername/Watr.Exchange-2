using ExRam.Gremlinq.Core;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Watr.Exchange.Data.Commands.Core;
using Watr.Exchange.Data.Core;

namespace Watr.Exchange.Data.Commands
{
    public abstract class CreateCommandHandler<TCommand, TVertex> : IRequestHandler<TCommand, Unit>
        where TVertex : IVertex, new()
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
