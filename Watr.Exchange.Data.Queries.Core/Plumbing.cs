using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Watr.Exchange.Data.Core;
using Watr.Exchange.Core;

namespace Watr.Exchange.Data.Queries.Core
{
    public class GetVertexById<TVertex> : IRequest<TVertex?>
        where TVertex: IVertex
    {
        public string Id { get; }
        public GetVertexById(string id)
        {
            Id = id;
        }
    }
    public class GetVertexes<TVertex> : IStreamRequest<TVertex>
        where TVertex : IVertex
    {
        public Pager? Pager { get; }
        public Expression<Func<TVertex, bool>>? Filter { get; }
        public Expression<Func<IQueryable<TVertex>, IOrderedQueryable<TVertex>>>? OrderBy { get; }
        public GetVertexes(Pager? pager = null, Expression<Func<TVertex, bool>>? filter = null, 
            Expression<Func<IQueryable<TVertex>, IOrderedQueryable<TVertex>>>? orderBy = null)
        {
            Pager = pager;
            Filter = filter;
            OrderBy = orderBy;
        }
    }
    public class GetQueryPageCount<TVertex> : IRequest<long>
        where TVertex : IVertex
    {
        public GetVertexes<TVertex> Query { get; }
        public GetQueryPageCount(GetVertexes<TVertex> query)
        {
            Query = query;
        }
    }
    
    public class GetSingleEdgeById<TEdge, TFrom, TTo> : IRequest<TEdge?>
        where TFrom: IVertex
        where TTo: IVertex
        where TEdge : ISingleEdge<TFrom, TTo>
    {
        public string FromId { get; }
        public GetSingleEdgeById(string fromId)
        {
            FromId = fromId;
        }
    }
    public class GetMultiEdgeById<TEdgeValue, TEdge, TFrom, TTo> : IStreamRequest<TEdgeValue>
        where TFrom : IVertex
        where TTo : IVertex
        where TEdgeValue : IEdgeValue<TFrom, TTo>
        where TEdge : IMultiEdge<TEdgeValue, TFrom, TTo>
    {
        public string FromId { get; }
        public GetMultiEdgeById(string fromId)
        {
            FromId = fromId;
        }
    }
}
