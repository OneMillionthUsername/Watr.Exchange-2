using Serialize.Linq.Serializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Watr.Exchange.Core;

namespace Watr.Exchange.Business.REST
{
    public class FilterRequest<TObject>
        where TObject : IObject
    {
        public string? FilterString { get; set; }
        public void SerializeFilter(Expression<Func<TObject, bool>> filter)
        {
            FilterString = SerializeExpression(filter);
        }
        protected string SerializeExpression(Expression expression)
        {
            var serializer = new ExpressionSerializer(new JsonSerializer());
            return serializer.SerializeText(expression);
        }
        public Expression<Func<TObject, bool>>? DeserializeFilter()
        {
            if (FilterString == null)
                return null;
            return DeserializeExpression(FilterString) as Expression<Func<TObject, bool>>;
        }
        protected Expression? DeserializeExpression(string expression)
        {
            var serializer = new ExpressionSerializer(new JsonSerializer());
            var exp = serializer.DeserializeText(FilterString);
            return exp;
        }
    }
    public class QueryRequest<TObject> : FilterRequest<TObject>
        where TObject : IObject
    {
       
        public string? OrderByString { get; set; }
        public Pager? Pager { get; set; }
        
        public void SerializeOrderBy(Expression<Func<IQueryable<TObject>, IOrderedQueryable<TObject>>> orderBy)
        {
            OrderByString = SerializeExpression(orderBy);
        }
       
        public Expression<Func<IQueryable<TObject>, IOrderedQueryable<TObject>>>? DeserializeOrderBy()
        {
            if (OrderByString == null) return null;
            return DeserializeExpression(OrderByString) as Expression<Func<IQueryable<TObject>, IOrderedQueryable<TObject>>>;
        }
        
    }
}
