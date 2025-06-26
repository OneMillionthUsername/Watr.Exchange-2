using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Watr.Exchange.Core
{
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
}
