using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Watr.Exchange.Business;
using Watr.Exchange.Business.Core;
using Watr.Exchange.Core;

namespace Watr.Exchange.Web.REST.Controllers
{
    [Route("api/actor")]
    public class ActorController : ReadUpdateDeleteController<IReadActorProcessor, IUpdateActorProcessor, IDeleteActorProcessor,
        IActorReadActivity, IUpdateActorActivity, IDeleteActorActivity,
        ReadActorDTO, UpdateActorDTO, DeleteActorDTO, Guid, IActor>
    {
        public ActorController(IReadActorProcessor readProcessor, IUpdateActorProcessor updateProcessor, IDeleteActorProcessor deleteProcessor, ILogger<IActor> logger) : base(readProcessor, updateProcessor, deleteProcessor, logger)
        {
        }
    }
    [Route("api/actor/admin")]
    public class AdminController : CreateReadUpdateDeleteController<ICreateAdminProcessor, IReadAdminProcessor, IUpdateAdminProcessor,
        IDeleteAdminProcessor, ICreateAdminActivity, IAdminReadActivity, IUpdateAdminActivity, IDeleteAdminActivity,
        CreateAdminDTO, ReadAdminDTO, UpdateAdminDTO, DeleteAdminDTO, Guid, IAdmin>
    {
        public AdminController(ICreateAdminProcessor createProcessor, IReadAdminProcessor readProcessor, IUpdateAdminProcessor updateProcessor, IDeleteAdminProcessor deleteProcessor, ILogger<IAdmin> logger) : base(createProcessor, readProcessor, updateProcessor, deleteProcessor, logger)
        {
        }
    }
}
