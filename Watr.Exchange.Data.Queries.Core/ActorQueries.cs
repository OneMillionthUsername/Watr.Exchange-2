using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Watr.Exchange.Data.Core;

namespace Watr.Exchange.Data.Queries.Core
{
    public sealed class GetActorById : GetVertexById<Actor>
    {
        public GetActorById(string id) : base(id)
        {
        }
    }
    public sealed class GetActors: GetVertexes<Actor>
    {
        public GetActors(Pager? pager, Expression<Func<Actor, bool>>? filter = null,
            Expression<Func<IQueryable<Actor>, IOrderedQueryable<Actor>>>? orderBy = null) : base(pager, filter ,orderBy) { }
    }
}
