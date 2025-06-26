using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Watr.Exchange.Core;

namespace Watr.Exchange.Business.Core
{
    public interface IProcessor
    {
    }
    public interface IProcessor<TKey> : IProcessor
        where TKey : IEquatable<TKey>
    {

    }
    public interface IProcessor<TKey, TObject> : IProcessor<TKey>
        where TKey : IEquatable<TKey>
        where TObject : IObject
    {

    }
    public interface ICreateProcessor<in TCreateDTO, TReadDTO, TKey, TObject> : IProcessor<TKey, TObject>
        where TKey : IEquatable<TKey>
        where TObject : IObject
        where TCreateDTO : ICreateDTO, TObject
        where TReadDTO : IReadDTO<TKey>, TObject
    {
        ICreateActivity<TCreateDTO, TReadDTO, TKey, TObject> CreateRaw { get; }
    }
    public interface IReadProcessor<TReadDTO, TKey, TObject> : IProcessor<TKey, TObject>
        where TKey : IEquatable<TKey>
        where TObject : IObject
        where TReadDTO : IReadDTO<TKey>, TObject
    {
        IReadActivity<TReadDTO, TKey, TObject> ReadRaw { get; }
    }
    public interface IUpdateProcessor<in TUpdateDTO, TReadDTO, TKey, TObject> : IProcessor<TKey, TObject>
        where TKey : IEquatable<TKey>
        where TObject : IObject
        where TReadDTO : IReadDTO<TKey>, TObject
        where TUpdateDTO : IUpdateDTO<TKey>, TObject
    {
        IUpdateActivity<TUpdateDTO, TReadDTO, TKey, TObject> UpdateRaw { get; }
    }
    public interface IDeleteProcessor<in TDeleteDTO, TKey, TObject> : IProcessor<TKey, TObject>
        where TKey : IEquatable<TKey>
        where TObject : IObject
        where TDeleteDTO : IDeleteDTO<TKey>
    {
        IDeleteActivity<TDeleteDTO, TKey, TObject> DeleteRaw { get; }
    }
    public interface ICRUDProcessor<in TCreateDTO, TReadDTO, in TUpdateDTO, in TDeleteDTO, TKey, TObject> :
        ICreateProcessor<TCreateDTO, TReadDTO, TKey, TObject>,
        IReadProcessor<TReadDTO, TKey, TObject>,
        IUpdateProcessor<TUpdateDTO, TReadDTO, TKey, TObject>,
        IDeleteProcessor<TDeleteDTO, TKey, TObject>
        where TKey : IEquatable<TKey>
        where TObject : IObject
        where TCreateDTO : ICreateDTO, TObject
        where TReadDTO : IReadDTO<TKey>, TObject
        where TUpdateDTO : IUpdateDTO<TKey>, TObject
        where TDeleteDTO : IDeleteDTO<TKey>
    {

    }
    public interface ICreateProcessor<out TCreateActivity, in TCreateDTO, TReadDTO, TKey, TObject>
    : ICreateProcessor<TCreateDTO, TReadDTO, TKey, TObject>
    where TKey : IEquatable<TKey>
    where TObject : IObject
    where TCreateDTO : ICreateDTO, TObject
    where TReadDTO : IReadDTO<TKey>, TObject
    where TCreateActivity : ICreateActivity<TCreateDTO, TReadDTO, TKey, TObject>
    {
        /// <summary>
        ///   A strongly‐typed alias for CreateRaw.
        /// </summary>
        TCreateActivity Create { get; }
    }

    public interface IReadProcessor<out TReadActivity, TReadDTO, TKey, TObject>
        : IReadProcessor<TReadDTO, TKey, TObject>
        where TKey : IEquatable<TKey>
        where TObject : IObject
        where TReadDTO : IReadDTO<TKey>, TObject
        where TReadActivity : IReadActivity<TReadDTO, TKey, TObject>
    {
        TReadActivity Read { get; }
    }

    public interface IUpdateProcessor<out TUpdateActivity, in TUpdateDTO, TReadDTO, TKey, TObject>
        : IUpdateProcessor<TUpdateDTO, TReadDTO, TKey, TObject>
        where TKey : IEquatable<TKey>
        where TObject : IObject
        where TReadDTO : IReadDTO<TKey>, TObject
        where TUpdateDTO : IUpdateDTO<TKey>, TObject
        where TUpdateActivity : IUpdateActivity<TUpdateDTO, TReadDTO, TKey, TObject>
    {
        TUpdateActivity Update { get; }
    }

    public interface IDeleteProcessor<out TDeleteActivity, in TDeleteDTO, TKey, TObject>
        : IDeleteProcessor<TDeleteDTO, TKey, TObject>
        where TKey : IEquatable<TKey>
        where TObject : IObject
        where TDeleteDTO : IDeleteDTO<TKey>
        where TDeleteActivity : IDeleteActivity<TDeleteDTO, TKey, TObject>
    {
        TDeleteActivity Delete { get; }
    }

    public interface ICRUDProcessor<
            out TCreateActivity, out TReadActivity, out TUpdateActivity, out TDeleteActivity,
            in TCreateDTO, TReadDTO, in TUpdateDTO, in TDeleteDTO,
            TKey, TObject> : ICRUDProcessor<TCreateDTO, TReadDTO, TUpdateDTO, TDeleteDTO, TKey, TObject>,
        ICreateProcessor<TCreateActivity, TCreateDTO, TReadDTO, TKey, TObject>,
        IReadProcessor<TReadActivity, TReadDTO, TKey, TObject>,
        IUpdateProcessor<TUpdateActivity, TUpdateDTO, TReadDTO, TKey, TObject>,
        IDeleteProcessor<TDeleteActivity, TDeleteDTO, TKey, TObject>
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
        // Composite interface; no new members
    }
}
