using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Watr.Exchange.Core
{
    public interface IAccount : IObject
    {
        string? FirstName { get; set; }
        string? LastName { get; set; }
        string ObjectId { get; set; }
    }
    public interface IRole
    {
        string Name { get; set; }
    }
}
