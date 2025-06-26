using ExRam.Gremlinq.Core;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Watr.Exchange.Data.Core;
using Watr.Exchange.Data.Queries.Core;

namespace Watr.Exchange.Data.Queries
{
    public class GetActorByIdHandler : GetVertexByIdHandler<GetActorById, Actor>
    {
        public GetActorByIdHandler(IGremlinQuerySource g, ILogger<GetActorById> logger) : base(g, logger)
        {
        }
    }
    public class GetActorsHandler : GetVertexesHandler<GetActors, Actor>
    {
        public GetActorsHandler(IGremlinQuerySource g, ILogger<GetVertexesHandler<GetActors, Actor>> logger) : base(g, logger)
        {
        }
    }
}
