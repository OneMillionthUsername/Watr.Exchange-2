using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Watr.Exchange.Business.Core;
using Watr.Exchange.Core;
using Watr.Exchange.DTO;

namespace Watr.Exchange.Business
{
    public class CreateAdminProcessor : CreateProcessor<ICreateAdminActivity, CreateAdminDTO, ReadAdminDTO, Guid, IAdmin>, ICreateAdminProcessor
    {
        public CreateAdminProcessor(ILogger<ICreateAdminActivity> logger, ICreateAdminActivity activity) : base(logger, activity)
        {
        }
    }
    public class ReadActorProcessor : ReadProcessor<IActorReadActivity, ReadActorDTO, Guid, IActor>, IReadActorProcessor
    {
        public ReadActorProcessor(ILogger<IActorReadActivity> logger, IActorReadActivity activity) : base(logger, activity)
        {
        }
    }
    public class ReadAdminProcessor : ReadProcessor<IAdminReadActivity, ReadAdminDTO, Guid, IAdmin>, IReadAdminProcessor
    {
        public ReadAdminProcessor(ILogger<IAdminReadActivity> logger, IAdminReadActivity activity) : base(logger, activity)
        {
        }
    }
    public class UpdateActorProcessor : UpdateProcessor<IUpdateActorActivity, UpdateGenericActorDTO, ReadActorDTO, Guid, IActor>, IUpdateActorProcessor
    {
        public UpdateActorProcessor(ILogger<IUpdateActorActivity> logger, IUpdateActorActivity activity) : base(logger, activity)
        {
        }
    }
    public class UpdateAdminProcessor : UpdateProcessor<IUpdateAdminActivity, UpdateAdminDTO, ReadAdminDTO, Guid, IAdmin>, IUpdateAdminProcessor
    {
        public UpdateAdminProcessor(ILogger<IUpdateAdminActivity> logger, IUpdateAdminActivity activity) : base(logger, activity)
        {
        }
    }
    public class DeleteActorProcessor : DeleteProcessor<IDeleteActorActivity, DeleteActorDTO, Guid, IActor>, IDeleteActorProcessor
    {
        public DeleteActorProcessor(ILogger<IDeleteActorActivity> logger, IDeleteActorActivity activity) : base(logger, activity)
        {
        }
    }
    public class DeleteAdminProcessor : DeleteProcessor<IDeleteAdminActivity, DeleteAdminDTO, Guid, IAdmin>, IDeleteAdminProcessor
    {
        public DeleteAdminProcessor(ILogger<IDeleteAdminActivity> logger, IDeleteAdminActivity activity) : base(logger, activity)
        {
        }
    }
    public class AdminCRUDProcessor : CRUDProcessorProxy<ICreateAdminProcessor, IReadAdminProcessor, IUpdateAdminProcessor, IDeleteAdminProcessor,
        ICreateAdminActivity, IAdminReadActivity, IUpdateAdminActivity, IDeleteAdminActivity,
        CreateAdminDTO, ReadAdminDTO, UpdateAdminDTO, DeleteAdminDTO, Guid, IAdmin>, ICRUDAdminProcessor
    {
        public AdminCRUDProcessor(ILogger logger, ICreateAdminProcessor createProc, IReadAdminProcessor readProc, IUpdateAdminProcessor updateProc, IDeleteAdminProcessor deleteProc) : base(logger, createProc, readProc, updateProc, deleteProc)
        {
        }
    }
}
