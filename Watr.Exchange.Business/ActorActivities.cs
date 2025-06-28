using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Watr.Exchange.Business.Core;
using Watr.Exchange.Core;
using Watr.Exchange.Data.Commands.Core;
using Watr.Exchange.Data.Core.Actors;
using Watr.Exchange.Data.Queries.Core;
using Watr.Exchange.DTO;

namespace Watr.Exchange.Business
{
    public class CreateAdminActivity : GuidCreateActivity<CreateAdminDTO, ReadAdminDTO, IAdmin, Admin>, ICreateAdminActivity
    {
        public CreateAdminActivity(ILogger<IAdmin> logger, IMediator mediator, IMapper mapper, IReadActivity<ReadAdminDTO, Guid, IAdmin> activity) : base(logger, mediator, mapper, activity)
        {
        }

        protected override CreateVertex<Admin> CreateQuery(Admin vertext)
        {
            return new CreateAdmin(vertext);
        }
    }
    public class ReadActorActivity : GuidReadActivity<ReadActorDTO, IActor, Actor>, IActorReadActivity
    {
        public ReadActorActivity(ILogger<IActor> logger, IMediator mediator, IMapper mapper) : base(logger, mediator, mapper)
        {
        }
    }
    public class ReadAdminActivity : GuidReadActivity<ReadAdminDTO, IAdmin, Admin>, IAdminReadActivity
    {
        public ReadAdminActivity(ILogger<IAdmin> logger, IMediator mediator, IMapper mapper) : base(logger, mediator, mapper)
        {
        }
    }
    public class UpdateActorAcitivity : GuidUpdateActivity<UpdateGenericActorDTO, ReadActorDTO, IActor, Actor>, IUpdateActorActivity
    {
        public UpdateActorAcitivity(ILogger<IActor> logger, IMediator mediator, IMapper mapper, IReadActivity<ReadActorDTO, Guid, IActor> read) : base(logger, mediator, mapper, read)
        {
        }
    }
    public class UpdateAdminActivity : GuidUpdateActivity<UpdateAdminDTO, ReadAdminDTO, IAdmin, Admin>, IUpdateAdminActivity
    {
        public UpdateAdminActivity(ILogger<IAdmin> logger, IMediator mediator, IMapper mapper, IReadActivity<ReadAdminDTO, Guid, IAdmin> read) : base(logger, mediator, mapper, read)
        {
        }
    }
    public class DeleteActorActivity : GuidDeleteActivity<DeleteActorDTO, IActor, Actor>, IDeleteActorActivity
    {
        public DeleteActorActivity(ILogger<IActor> logger, IMediator mediator, IMapper mapper) : base(logger, mediator, mapper)
        {
        }
    }
    public class DeleteAdminActivity : GuidDeleteActivity<DeleteAdminDTO, IAdmin, Admin>, IDeleteAdminActivity
    {
        public DeleteAdminActivity(ILogger<IAdmin> logger, IMediator mediator, IMapper mapper) : base(logger, mediator, mapper)
        {
        }
    }
}
