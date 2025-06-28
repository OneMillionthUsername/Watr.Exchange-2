using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Watr.Exchange.Core;

namespace Watr.Exchange.Business.Core
{
    public interface IActorReadActivity : IReadActivity<ReadActorDTO, Guid, IActor> { }
    public interface IAdminReadActivity : IReadActivity<ReadAdminDTO, Guid, IAdmin> { }
    public interface ICreateAdminActivity : ICreateActivity<CreateAdminDTO, ReadAdminDTO, Guid, IAdmin> { }
    public interface IUpdateActorActivity : IUpdateActivity<UpdateActorDTO, ReadActorDTO, Guid, IActor> { }
    public interface IUpdateAdminActivity : IUpdateActivity<UpdateAdminDTO, ReadAdminDTO, Guid, IAdmin> { }
    public interface IDeleteActorActivity : IDeleteActivity<DeleteActorDTO, Guid, IActor> { }
    public interface IDeleteAdminActivity : IDeleteActivity<DeleteAdminDTO, Guid, IAdmin> { }
}
