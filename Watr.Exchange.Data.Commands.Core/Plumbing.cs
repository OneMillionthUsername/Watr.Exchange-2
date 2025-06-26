using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Watr.Exchange.Data.Core;

namespace Watr.Exchange.Data.Commands.Core
{
    public abstract class CreateVertex<TVertex> : IRequest<Unit>
        where TVertex : IVertex, new()
    {
        public TVertex Vertex { get; }
        public CreateVertex(TVertex vertex)
        {
            Vertex = vertex;
        }
    }
    public class UpdateVertex<TVertex> : IRequest<Unit>
        where TVertex : IVertex
    {
        public TVertex Vertex { get; }
        public UpdateVertex(TVertex vertex)
        {
            Vertex = vertex;
        }
    }
    public class DeleteVertex : IRequest<Unit>
    {
        public string Id { get; }
        public DeleteVertex(string id)
        {
            Id = id;
        }
    }
}
