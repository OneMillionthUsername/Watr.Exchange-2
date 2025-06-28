using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Watr.Exchange.Core;

namespace Watr.Exchange.Data.Core.Accounts
{
    public class Account : Vertex, IAccount
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string ObjectId { get; set; } = null!;
        [JsonIgnore]
        public HasRoles? __Roles { get; set; }
        [JsonIgnore]
        public EmailAddress? __EmailAddress { get; set; }
    }
    public class Role : Vertex, IRole
    {
        public string Name { get; set; } = null!;
    }
    public class HasRole : EdgeValue<Account, Role> { }
    public class HasRoles : MultiEdge<HasRole, Account, Role>
    {
      
    }
    public class EmailAddress : SingleEdge<Account, ContactMechanisms.EmailAddress>
    {

    }
}
