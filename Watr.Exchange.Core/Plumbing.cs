using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Watr.Exchange.Core
{
    [AttributeUsage(AttributeTargets.Property)]
    public class MapIgnoreAttribute : Attribute
    {
    }
    public interface IConcrete { }

    public interface IGeneric : IConcrete { }

    internal static class IgnoredValues
    {
        public const string STRING____IGNORE___VALUE = nameof(STRING____IGNORE___VALUE);
    }
    public static class StringIgnore
    {
        public const string Ignore = IgnoredValues.STRING____IGNORE___VALUE;
    }
    public interface IObject
    {

    }
    public interface ISpecification<TType> : IObject
        where TType : struct, Enum
    {
        TType Type { get; }
    }
    public interface ISpecification<TType, TStereotype> : ISpecification<TType>
        where TType: struct, Enum
        where TStereotype: struct, Enum
    {
        
        TStereotype Stereotype { get; }
    }
    public struct Pager
    {
        public int Size { get; init; }
        public int Page { get; init; }
    }
}
