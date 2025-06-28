using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Watr.Exchange.Core;

namespace Watr.Exchange.DTO
{
    public class CreateAccountDTO : CreateDTO, IAccount
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string ObjectId { get; set; } = null!;
    }
    public class CreateRoleDTO : CreateDTO, IRole
    {
        public string Name { get; set; } = null!;
    }
    public class ReadRoleDTO : ReadDTO<Guid>, IRole
    {
        public string Name { get; set; } = null!;
    }
    public class ReadAccountDTO : ReadDTO<Guid>, IAccount
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string ObjectId { get; set; } = null!;
        public ICollection<ReadRoleDTO> Roles { get; set; } = [];
    }
}
