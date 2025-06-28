using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Watr.Exchange.Data.Core;

namespace Watr.Exchange.Data.Commands.Core
{
    public abstract class CreateVertex<TVertex> : IRequest<string>
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
    public class HardDeleteVertex : DeleteVertex
    {
        public HardDeleteVertex(string id) : base(id)
        {
        }
    }
    public class CreateSingleEdge<TEdge, TFrom, TTo> : IRequest<string>
        where TFrom : IVertex
        where TTo : IVertex
        where TEdge : ISingleEdge<TFrom, TTo>, new()
    {
        public TEdge Edge { get; }
        public CreateSingleEdge(TEdge edge)
        {
            Edge = edge;
        }
    }
    public class CreateMultiEdge<TEdgeValue, TEdge, TFrom, TTo> : IStreamRequest<string>
        where TFrom : IVertex
        where TTo : IVertex
        where TEdgeValue: IEdgeValue<TFrom, TTo>
        where TEdge : IMultiEdge<TEdgeValue, TFrom, TTo>, new()
    {
        public TEdge Edge { get; }
        public CreateMultiEdge(TEdge edge)
        {
            Edge = edge;
        }
    }
    public class UpdateSingleEdge<TEdge, TFrom, TTo> : IRequest<string>
        where TFrom : IVertex
        where TTo : IVertex
        where TEdge : ISingleEdge<TFrom, TTo>, new()
    {
        public TEdge Edge { get; }
        public UpdateSingleEdge(TEdge edge)
        {
            Edge = edge;
        }
    }
    public class UpdateMultiEdge<TEdgeValue, TEdge, TFrom, TTo> : IStreamRequest<string>
        where TFrom : IVertex
        where TTo : IVertex
        where TEdgeValue: IEdgeValue<TFrom, TTo>
        where TEdge : IMultiEdge<TEdgeValue, TFrom, TTo>, new()
    {
        public TEdge Edge { get; }
        public UpdateMultiEdge(TEdge edge)
        {
            Edge = edge;
        }
    }
    public class DeleteEdge: IRequest<Unit>
    {
        public string Id { get; }
        public DeleteEdge(string id)
        {
            Id = id;
        }
    }
    public class HardDeleteEdge : DeleteEdge
    {
        public HardDeleteEdge(string id) : base(id)
        {
        }
    }
}
