using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Business.DTOs.Role
{
    public class RoleView
    {
        public Infrastructure.Models.Role? Roles { get; set; }
        public List<Infrastructure.Models.Permission> ListPermissions { get; set; }
    }
}
