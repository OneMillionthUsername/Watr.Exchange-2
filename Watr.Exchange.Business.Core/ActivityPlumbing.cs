using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Watr.Exchange.Core;
using Watr.Exchange.DTO;

namespace Watr.Exchange.Business.Core
{
    public interface IActivity
    {

    }
    public interface IActivity<TKey> : IActivity
        where TKey : IEquatable<TKey>
    {

    }
    public interface IActivity<TKey, TObject> : IActivity<TKey>
        where TKey : IEquatable<TKey>
        where TObject : IObject
    {

    }
    public interface ICommandActivity<TKey, TObject> : IActivity<TKey, TObject>
        where TKey : IEquatable<TKey>
        where TObject : IObject
    {

    }
    public interface IQueryActivity<TKey, TObject> : IActivity<TKey, TObject>
        where TKey : IEquatable<TKey>
        where TObject : IObject
    {
        Task<long> Count(Expression<Func<TObject, bool>>? filter = null, CancellationToken token = default);
    }
    public interface ICreateActivity<in TCreateDTO, TReadDTO, TKey, TObject> : ICommandActivity<TKey, TObject>
        where TKey : IEquatable<TKey>
        where TObject : IObject
        where TCreateDTO : ICreateDTO, TObject
        where TReadDTO : IReadDTO<TKey>, TObject
    {
        Task<TReadDTO> Create(TCreateDTO dto, CancellationToken token = default);
    }
    public interface IUpdateActivity<in TUpdateDTO, TReadDTO, TKey, TObject> : ICommandActivity<TKey, TObject>
        where TKey : IEquatable<TKey>
        where TObject : IObject
        where TReadDTO : IReadDTO<TKey>, TObject
        where TUpdateDTO : IUpdateDTO<TKey>, TObject
    {
        Task<TReadDTO> Update(TUpdateDTO dto, CancellationToken token = default);
    }
    public interface IDeleteActivity<in TDeleteDTO, TKey, TObject> : ICommandActivity<TKey, TObject>
        where TKey : IEquatable<TKey>
        where TObject : IObject
        where TDeleteDTO : IDeleteDTO<TKey>
    {
        Task Delete(TDeleteDTO dto, bool hardDrop = false, CancellationToken token = default);
    }
    public interface IReadActivity<TReadDTO, TKey, TObject> : IQueryActivity<TKey, TObject>
        where TKey : IEquatable<TKey>
        where TObject : IObject
        where TReadDTO : IReadDTO<TKey>, TObject
    {
        Task<TReadDTO?> GetByKey(TKey key, CancellationToken token = default);
        IAsyncEnumerable<TReadDTO> Get(Pager? pager = null, Expression<Func<TObject, bool>>? filter = null,
            Expression<Func<IQueryable<TObject>, IOrderedQueryable<TObject>>>? orderBy = null,
            CancellationToken token = default);
    }
}
