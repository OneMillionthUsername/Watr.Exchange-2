using ExRam.Gremlinq.Core;
using Gremlin.Net.Process.Traversal;
using Gremlin.Net.Structure;
using MediatR;
using Microsoft.Extensions.Logging;
using Remotion.Linq.Clauses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Watr.Exchange.Data.Core;
using Watr.Exchange.Data.Queries.Core;
using Watr.Exchange.Core;

namespace Watr.Exchange.Data.Queries
{
    public class GetVertexByIdHandler<TQuery, TVertex> : IRequestHandler<TQuery, TVertex?>
        where TVertex : IVertex
        where TQuery : GetVertexById<TVertex>
    {
        protected IGremlinQuerySource G { get; }
        protected ILogger Logger { get; }
        public GetVertexByIdHandler(IGremlinQuerySource g, ILogger<TQuery> logger)
        {
            G = g;
            Logger = logger;
        }
        public virtual async Task<TVertex?> Handle(TQuery request, CancellationToken cancellationToken)
        {
            try
            {
                return await G.V(request.Id).OfType<TVertex>().FirstOrDefaultAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
    internal static class Extensions
    {
        public static IVertexGremlinQuery<TVertex> BuildBaseQuery<TVertex>(this IVertexGremlinQuery<TVertex> query, Pager? pager = null, Expression<Func<TVertex, bool>>? filter = null,
            Expression<Func<IQueryable<TVertex>, IOrderedQueryable<TVertex>>>? orderBy = null)
            where TVertex : IVertex
        {
            if (filter != null)
                query = query.Where(filter);
            if (orderBy != null)
                query = query.Order(builder => builder.ApplyOrder(orderBy));
            if (pager != null)
            {
                var p = pager.Value;
                int skip = p.Size * (p.Page - 1);
                query = query.Skip(skip).Limit(p.Size);
            }
            return query;
        }

    }
    public class GetVertexesHandler<TQuery, TVertex> : IStreamRequestHandler<TQuery, TVertex>
        where TVertex : IVertex
        where TQuery : GetVertexes<TVertex>
    {
        protected IGremlinQuerySource G { get; }
        protected ILogger Logger { get; }
        public GetVertexesHandler(IGremlinQuerySource g, ILogger<GetVertexesHandler<TQuery, TVertex>> logger)
        {
            G = g;
            Logger = logger;
        }
        public IAsyncEnumerable<TVertex> Handle(TQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = G.V<TVertex>().BuildBaseQuery(request.Pager, request.Filter, request.OrderBy);
                return query.ToAsyncEnumerable();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
    public class GetQueryPageCountHandler<TQuery, TVertex> : IRequestHandler<TQuery, long>
        where TVertex: IVertex
        where TQuery: GetQueryPageCount<TVertex>
    {
        protected IGremlinQuerySource G { get; }
        protected ILogger Logger { get; }
        public GetQueryPageCountHandler(IGremlinQuerySource g, ILogger<TQuery> logger)
        {
            G = g;
            Logger = logger;
        }

        public async Task<long> Handle(TQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = G.V<TVertex>().BuildBaseQuery(filter: request.Query.Filter);
                return await query.Count().FirstAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
    internal static class OrderExpressionConverter
    {
        /// <summary>
        /// Applies a LINQ ordering expression to a Gremlinq IOrderBuilder.
        /// </summary>
        public static IOrderBuilderWithBy<TElement, TSourceQuery> ApplyOrder<TElement, TSourceQuery>(
            this IOrderBuilder<TElement, TSourceQuery> builder,
            Expression<Func<IQueryable<TElement>, IOrderedQueryable<TElement>>> orderExpr)
            where TSourceQuery : IGremlinQueryBase<TElement>
        {
            // Extract ordering operations
            var visitor = new OrderByExpressionVisitor<TElement>();
            visitor.Visit(orderExpr.Body);

            // Apply each ordering to the Gremlin builder
            IOrderBuilderWithBy<TElement, TSourceQuery> result = null!;
            bool first = true;
            foreach (var op in visitor.Orderings)
            {
                if (first)
                {
                    result = op.Descending
                        ? builder.ByDescending(op.KeySelector)
                        : builder.By(op.KeySelector);
                    first = false;
                }
                else
                {
                    result = op.Descending
                        ? result.ByDescending(op.KeySelector)
                        : result.By(op.KeySelector);
                }
            }
            return result;
        }

        private class OrderByExpressionVisitor<T>
            : ExpressionVisitor
        {
            public struct Ordering
            {
                public Expression<Func<T, object?>> KeySelector;
                public bool Descending;
            }

            public List<Ordering> Orderings { get; } = new();

            protected override Expression VisitMethodCall(MethodCallExpression node)
            {
                if (node.Method.DeclaringType == typeof(Queryable) &&
                    (node.Method.Name == nameof(Queryable.OrderBy) ||
                     node.Method.Name == nameof(Queryable.OrderByDescending) ||
                     node.Method.Name == nameof(Queryable.ThenBy) ||
                     node.Method.Name == nameof(Queryable.ThenByDescending)))
                {
                    // Extract key selector
                    var lambda = (LambdaExpression)StripQuotes(node.Arguments[1]);
                    var converted = Expression.Lambda<Func<T, object?>>(
                        Expression.Convert(lambda.Body, typeof(object)),
                        lambda.Parameters);

                    // Prepend to preserve original order (OrderBy before ThenBy)
                    Orderings.Insert(0, new Ordering
                    {
                        KeySelector = converted,
                        Descending = node.Method.Name.EndsWith("Descending", StringComparison.Ordinal)
                    });

                    // Recurse into inner call
                    Visit(node.Arguments[0]);
                    return node;
                }

                return base.VisitMethodCall(node);
            }

            private static Expression StripQuotes(Expression e)
            {
                while (e is UnaryExpression ue && ue.NodeType == ExpressionType.Quote)
                    e = ue.Operand;
                return e;
            }
        }
    }
    
}