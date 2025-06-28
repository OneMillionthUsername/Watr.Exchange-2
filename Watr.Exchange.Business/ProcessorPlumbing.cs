using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Watr.Exchange.Business.Core;
using Watr.Exchange.Core;
using Watr.Exchange.DTO;

namespace Watr.Exchange.Business
{
    public abstract class Processor : IProcessor
    {
        protected ILogger Logger { get; }
        public Processor(ILogger logger)
        {
            Logger = logger;
        }
    }
    public abstract class Processor<TKey> : Processor, IProcessor<TKey>
        where TKey : IEquatable<TKey>
    {
        protected Processor(ILogger logger) : base(logger)
        {
        }
    }
    public abstract class Processor<TKey, TObject> : Processor<TKey>, IProcessor<TKey, TObject>
        where TKey : IEquatable<TKey>
        where TObject : IObject
    {
        protected Processor(ILogger logger) : base(logger)
        {
        }
    }
    public abstract class ActivityProcessor<TActivity, TKey, TObject> : Processor<TKey, TObject>
         where TKey : IEquatable<TKey>
         where TObject : IObject
         where TActivity : IActivity<TKey, TObject>
    {
        protected TActivity Activity { get; }
        protected ActivityProcessor(ILogger<TActivity> logger, TActivity activity) : base(logger)
        {
            Activity = activity;
        }
    }
    public class CreateProcessor<TCreateActivity, TCreateDTO, TReadDTO, TKey, TObject>
        : ActivityProcessor<TCreateActivity, TKey, TObject>, ICreateProcessor<TCreateActivity, TCreateDTO, TReadDTO, TKey, TObject>, IConcrete
        where TKey : IEquatable<TKey>
        where TObject : IObject
        where TCreateDTO : ICreateDTO, TObject
        where TReadDTO : IReadDTO<TKey>, TObject
        where TCreateActivity : ICreateActivity<TCreateDTO, TReadDTO, TKey, TObject>
    {
        public CreateProcessor(ILogger<TCreateActivity> logger, TCreateActivity activity) : base(logger, activity)
        {
        }

        public virtual TCreateActivity Create => Activity;

        public virtual ICreateActivity<TCreateDTO, TReadDTO, TKey, TObject> CreateRaw => Create;
    }
    public class ReadProcessor<TReadActivity, TReadDTO, TKey, TObject> :
        ActivityProcessor<TReadActivity, TKey, TObject>, IReadProcessor<TReadActivity, TReadDTO, TKey, TObject>, IConcrete
        where TKey : IEquatable<TKey>
        where TObject : IObject
        where TReadDTO : IReadDTO<TKey>, TObject
        where TReadActivity : IReadActivity<TReadDTO, TKey, TObject>
    {
        public ReadProcessor(ILogger<TReadActivity> logger, TReadActivity activity) : base(logger, activity)
        {
        }

        public virtual TReadActivity Read => Activity;

        public virtual IReadActivity<TReadDTO, TKey, TObject> ReadRaw => Read;
    }
    public class UpdateProcessor<TUpdateActivity, TUpdateDTO, TReadDTO, TKey, TObject> :
        ActivityProcessor<TUpdateActivity, TKey, TObject>,
        IUpdateProcessor<TUpdateActivity, TUpdateDTO, TReadDTO, TKey, TObject>, IConcrete
        where TKey : IEquatable<TKey>
        where TObject : IObject
        where TReadDTO : IReadDTO<TKey>, TObject
        where TUpdateDTO : IUpdateDTO<TKey>, TObject
        where TUpdateActivity : IUpdateActivity<TUpdateDTO, TReadDTO, TKey, TObject>
    {
        public UpdateProcessor(ILogger<TUpdateActivity> logger, TUpdateActivity activity) : base(logger, activity)
        {
        }

        public virtual TUpdateActivity Update => Activity;

        public virtual IUpdateActivity<TUpdateDTO, TReadDTO, TKey, TObject> UpdateRaw => Update;
    }
    public class DeleteProcessor<TDeleteActivity, TDeleteDTO, TKey, TObject> : ActivityProcessor<TDeleteActivity, TKey, TObject>,
        IDeleteProcessor<TDeleteActivity, TDeleteDTO, TKey, TObject>, IConcrete
        where TKey : IEquatable<TKey>
        where TObject : IObject
        where TDeleteDTO : IDeleteDTO<TKey>
        where TDeleteActivity : IDeleteActivity<TDeleteDTO, TKey, TObject>
    {
        public DeleteProcessor(ILogger<TDeleteActivity> logger, TDeleteActivity activity) : base(logger, activity)
        {
        }

        public virtual TDeleteActivity Delete => Activity;

        public virtual IDeleteActivity<TDeleteDTO, TKey, TObject> DeleteRaw => Delete;
    }

    public class CRUDProcessorProxy<TCreateProcessor, TReadProcessor, TUpdateProcessor, TDeleteProcessor,
        TCreateActivity, TReadActivity, TUpdateActivity, TDeleteActivity,
        TCreateDTO, TReadDTO, TUpdateDTO, TDeleteDTO, TKey, TObject> : Processor<TKey, TObject>,
        ICRUDProcessor<TCreateActivity, TReadActivity, TUpdateActivity, TDeleteActivity,
            TCreateDTO, TReadDTO, TUpdateDTO, TDeleteDTO, TKey, TObject>, IConcrete
        where TCreateProcessor: ICreateProcessor<TCreateActivity, TCreateDTO, TReadDTO, TKey, TObject>
        where TReadProcessor : IReadProcessor<TReadActivity, TReadDTO, TKey, TObject>
        where TUpdateProcessor: IUpdateProcessor<TUpdateActivity, TUpdateDTO, TReadDTO, TKey, TObject>
        where TDeleteProcessor: IDeleteProcessor<TDeleteActivity, TDeleteDTO, TKey, TObject>
        where TCreateActivity : ICreateActivity<TCreateDTO, TReadDTO, TKey, TObject>
        where TReadActivity : IReadActivity<TReadDTO, TKey, TObject>
        where TUpdateActivity : IUpdateActivity<TUpdateDTO, TReadDTO, TKey, TObject>
        where TDeleteActivity : IDeleteActivity<TDeleteDTO, TKey, TObject>
        where TKey : IEquatable<TKey>
        where TObject : IObject
        where TCreateDTO : ICreateDTO, TObject
        where TReadDTO : IReadDTO<TKey>, TObject
        where TUpdateDTO : IUpdateDTO<TKey>, TObject
        where TDeleteDTO : IDeleteDTO<TKey>
    {
        protected TCreateProcessor CreateProc { get; }
        protected TReadProcessor ReadProc { get; }
        protected TUpdateProcessor UpdateProc { get; }
        protected TDeleteProcessor DeleteProc { get; }
        public CRUDProcessorProxy(ILogger logger, TCreateProcessor createProc, TReadProcessor readProc, TUpdateProcessor updateProc, TDeleteProcessor deleteProc) : base(logger)
        {
            CreateProc = createProc;
            ReadProc = readProc;
            UpdateProc = updateProc;
            DeleteProc = deleteProc;
        }

        public virtual TCreateActivity Create => CreateProc.Create;

        public virtual ICreateActivity<TCreateDTO, TReadDTO, TKey, TObject> CreateRaw => CreateProc.CreateRaw;

        public virtual TReadActivity Read => ReadProc.Read;

        public virtual IReadActivity<TReadDTO, TKey, TObject> ReadRaw => ReadProc.ReadRaw;

        public virtual TUpdateActivity Update => UpdateProc.Update;

        public virtual IUpdateActivity<TUpdateDTO, TReadDTO, TKey, TObject> UpdateRaw => UpdateProc.UpdateRaw;

        public virtual TDeleteActivity Delete => DeleteProc.Delete;

        public virtual IDeleteActivity<TDeleteDTO, TKey, TObject> DeleteRaw => DeleteProc.DeleteRaw;
    }
    public class CRUDProcessorProxy<TCreateActivity, TReadActivity, TUpdateActivity, TDeleteActivity,
        TCreateDTO, TReadDTO, TUpdateDTO, TDeleteDTO, TKey, TObject> :
        CRUDProcessorProxy<CreateProcessor<TCreateActivity, TCreateDTO, TReadDTO, TKey, TObject>,
            ReadProcessor<TReadActivity, TReadDTO, TKey, TObject>,
            UpdateProcessor<TUpdateActivity, TUpdateDTO, TReadDTO, TKey, TObject>,
            DeleteProcessor<TDeleteActivity, TDeleteDTO, TKey, TObject>,
        TCreateActivity, TReadActivity, TUpdateActivity, TDeleteActivity,
        TCreateDTO, TReadDTO, TUpdateDTO, TDeleteDTO, TKey, TObject>,
        ICRUDProcessor<TCreateActivity, TReadActivity, TUpdateActivity, TDeleteActivity,
            TCreateDTO, TReadDTO, TUpdateDTO, TDeleteDTO, TKey, TObject>, IConcrete
        where TCreateActivity : ICreateActivity<TCreateDTO, TReadDTO, TKey, TObject>
        where TReadActivity : IReadActivity<TReadDTO, TKey, TObject>
        where TUpdateActivity : IUpdateActivity<TUpdateDTO, TReadDTO, TKey, TObject>
        where TDeleteActivity : IDeleteActivity<TDeleteDTO, TKey, TObject>
        where TKey : IEquatable<TKey>
        where TObject : IObject
        where TCreateDTO : ICreateDTO, TObject
        where TReadDTO : IReadDTO<TKey>, TObject
        where TUpdateDTO : IUpdateDTO<TKey>, TObject
        where TDeleteDTO : IDeleteDTO<TKey>
    {
        public CRUDProcessorProxy(ILogger logger, CreateProcessor<TCreateActivity, TCreateDTO, TReadDTO, TKey, TObject> createProc, ReadProcessor<TReadActivity, TReadDTO, TKey, TObject> readProc, UpdateProcessor<TUpdateActivity, TUpdateDTO, TReadDTO, TKey, TObject> updateProc, DeleteProcessor<TDeleteActivity, TDeleteDTO, TKey, TObject> deleteProc) : base(logger, createProc, readProc, updateProc, deleteProc)
        {
        }
    }
}
