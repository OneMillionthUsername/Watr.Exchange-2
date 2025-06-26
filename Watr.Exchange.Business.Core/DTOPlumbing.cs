using ExRam.Gremlinq.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Watr.Exchange.Core;

namespace Watr.Exchange.Business.Core
{
    public interface IDTO { }
    public interface ICreateDTO : IDTO { }

    public interface IUpdateDTO<TKey> : IDTO
        where TKey : IEquatable<TKey>
    {
        TKey Id { get; set; }
    }

    public interface IDeleteDTO<TKey> : IDTO
        where TKey : IEquatable<TKey>
    {
        TKey Id { get; set; }
    }
    public interface IReadDTO<TKey> : IDTO
        where TKey : IEquatable<TKey>
    {
        TKey Id { get; set; }
        DateTime CreateDate { get; set; }
        DateTime UpdateDate { get; set; }
        Guid? CreatedByUserId { get; set; }
        Guid? UpdatedByUserId { get; set; }
    }
    public abstract class DTO : IDTO { }
    public abstract class CreateDTO : DTO, ICreateDTO { }
    public abstract class UpdateDTO<TKey> : DTO, IUpdateDTO<TKey>
    where TKey : IEquatable<TKey>
    {
        public TKey Id { get; set; } = default!;
    }
    public abstract class DeleteDTO<TKey> : DTO, IDeleteDTO<TKey>
    where TKey : IEquatable<TKey>
    {
        public TKey Id { get; set; } = default!;
    }
    public abstract class ReadDTO<TKey> : DTO, IReadDTO<TKey>
    where TKey : IEquatable<TKey>
    {
        public TKey Id { get; set; } = default!;
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public Guid? CreatedByUserId { get; set; }
        public Guid? UpdatedByUserId { get; set; }
    }
    
    
}
