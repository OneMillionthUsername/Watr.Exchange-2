using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Watr.Exchange.Business;
using Watr.Exchange.Business.Core;
using Watr.Exchange.Business.REST;
using Watr.Exchange.Core;

namespace Watr.Exchange.Web.REST.Controllers
{
    [ApiController]
    public abstract class ReadUpdateDeleteController<TReadProcessor, TUpdateProcessor, TDeleteProcessor,
        TReadActivity, TUpdateActivity, TDeleteActivity,
        TReadDTO, TUpdateDTO, TDeleteDTO, TKey, TObject> : ControllerBase
        where TReadProcessor : IReadProcessor<TReadActivity, TReadDTO, TKey, TObject>
        where TUpdateProcessor : IUpdateProcessor<TUpdateActivity, TUpdateDTO, TReadDTO, TKey, TObject>
        where TDeleteProcessor : IDeleteProcessor<TDeleteActivity, TDeleteDTO, TKey, TObject>
        where TReadActivity : IReadActivity<TReadDTO, TKey, TObject>
        where TUpdateActivity : IUpdateActivity<TUpdateDTO, TReadDTO, TKey, TObject>
        where TDeleteActivity : IDeleteActivity<TDeleteDTO, TKey, TObject>
        where TReadDTO : IReadDTO<TKey>, TObject
        where TUpdateDTO : IUpdateDTO<TKey>, TObject
        where TDeleteDTO : IDeleteDTO<TKey>
        where TKey : IEquatable<TKey>
        where TObject : IObject
    {
        protected TReadProcessor ReadProcessor { get; }
        protected TUpdateProcessor UpdateProcessor { get; }
        protected TDeleteProcessor DeleteProcessor { get; }
        protected ILogger Logger { get; }
        public ReadUpdateDeleteController(TReadProcessor readProcessor, TUpdateProcessor updateProcessor, TDeleteProcessor deleteProcessor, 
            ILogger<TObject> logger)
        {
            ReadProcessor = readProcessor;
            UpdateProcessor = updateProcessor;
            DeleteProcessor = deleteProcessor;
            Logger = logger;
        }
        [HttpPost("all")]
        public IAsyncEnumerable<TReadDTO> GetAll([FromBody] QueryRequest<TObject>? request, CancellationToken token)
        {
            try
            {
                var filter = request?.DeserializeFilter();
                var orderBy = request?.DeserializeOrderBy();
                return ReadProcessor.Read.Get(request?.Pager, filter, orderBy, token);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }
        [HttpGet("{id}")]
        public Task<TReadDTO?> Get(TKey id, CancellationToken token)
        {
            try
            {
                return ReadProcessor.Read.GetByKey(id, token);
            }
            catch(Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }
        [HttpPut]
        public Task<TReadDTO> Update([FromBody]TUpdateDTO dto, CancellationToken token)
        {
            try
            {
                return UpdateProcessor.Update.Update(dto, token);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }
        [HttpDelete]
        public Task Delete(TDeleteDTO dto, CancellationToken token)
        {
            try
            {
                return DeleteProcessor.Delete.Delete(dto, token: token);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }
        [HttpDelete("hard")]
        public Task DeleteHard(TDeleteDTO dto, CancellationToken token)
        {
            try
            {
                return DeleteProcessor.Delete.Delete(dto, true, token);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
    public abstract class CreateReadUpdateDeleteController<TCreateProcessor, TReadProcessor, TUpdateProcessor, TDeleteProcessor,
        TCreateActivity, TReadActivity, TUpdateActivity, TDeleteActivity,
        TCreateDTO, TReadDTO, TUpdateDTO, TDeleteDTO, TKey, TObject> :
        ReadUpdateDeleteController<TReadProcessor, TUpdateProcessor, TDeleteProcessor,
        TReadActivity, TUpdateActivity, TDeleteActivity,
        TReadDTO, TUpdateDTO, TDeleteDTO, TKey, TObject>
        where TCreateProcessor : ICreateProcessor<TCreateActivity, TCreateDTO, TReadDTO, TKey, TObject>
        where TReadProcessor : IReadProcessor<TReadActivity, TReadDTO, TKey, TObject>
        where TUpdateProcessor : IUpdateProcessor<TUpdateActivity, TUpdateDTO, TReadDTO, TKey, TObject>
        where TDeleteProcessor : IDeleteProcessor<TDeleteActivity, TDeleteDTO, TKey, TObject>
        where TCreateActivity : ICreateActivity<TCreateDTO, TReadDTO, TKey, TObject>
        where TReadActivity : IReadActivity<TReadDTO, TKey, TObject>
        where TUpdateActivity : IUpdateActivity<TUpdateDTO, TReadDTO, TKey, TObject>
        where TDeleteActivity : IDeleteActivity<TDeleteDTO, TKey, TObject>
        where TCreateDTO : ICreateDTO, TObject
        where TReadDTO : IReadDTO<TKey>, TObject
        where TUpdateDTO : IUpdateDTO<TKey>, TObject
        where TDeleteDTO : IDeleteDTO<TKey>
        where TKey : IEquatable<TKey>
        where TObject : IObject
    {
        protected TCreateProcessor CreateProcessor { get; }
        protected CreateReadUpdateDeleteController(TCreateProcessor createProcessor, TReadProcessor readProcessor,
            TUpdateProcessor updateProcessor, TDeleteProcessor deleteProcessor, ILogger<TObject> logger) :
            base(readProcessor, updateProcessor, deleteProcessor, logger)
        {
            CreateProcessor = createProcessor;
        }
        [HttpPost]
        public Task<TReadDTO> Create([FromBody] TCreateDTO dto, CancellationToken token)
        {
            try
            {
                return CreateProcessor.Create.Create(dto, token);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}
