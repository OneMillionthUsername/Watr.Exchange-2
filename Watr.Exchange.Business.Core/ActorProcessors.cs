using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Watr.Exchange.Core;

namespace Watr.Exchange.Business.Core
{
    public interface ICreateAdminProcessor : ICreateProcessor<ICreateAdminActivity, CreateAdminDTO, ReadAdminDTO, Guid, IAdmin> { }
    public interface IReadActorProcessor : IReadProcessor<IActorReadActivity, ReadActorDTO, Guid, IActor> { }
    public interface IReadAdminProcessor : IReadProcessor<IAdminReadActivity, ReadAdminDTO, Guid, IAdmin> { }
    public interface IUpdateActorProcessor : IUpdateProcessor<IUpdateActorActivity, UpdateActorDTO, ReadActorDTO, Guid, IActor> { }
    public interface IUpdateAdminProcessor : IUpdateProcessor<IUpdateAdminActivity, UpdateAdminDTO, ReadAdminDTO, Guid, IAdmin> { }
    public interface IDeleteActorProcessor : IDeleteProcessor<IDeleteActorActivity, DeleteActorDTO, Guid, IActor> { }
    public interface IDeleteAdminProcessor : IDeleteProcessor<IDeleteAdminActivity, DeleteAdminDTO, Guid, IAdmin> { }
    public interface ICRUDAdminProcessor : ICRUDProcessor<ICreateAdminActivity, IAdminReadActivity, IUpdateAdminActivity, IDeleteAdminActivity,
        CreateAdminDTO, ReadAdminDTO, UpdateAdminDTO, DeleteAdminDTO, Guid, IAdmin>
    { }
}
