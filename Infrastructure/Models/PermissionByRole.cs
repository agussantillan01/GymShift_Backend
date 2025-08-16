using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models
{
    public class PermissionByRole
    {
        public int IdRole { get; set; }
        public Role Role { get; set; }
        public int IdPermission { get; set; }
        public Permission Permission { get; set; }
    }
}
