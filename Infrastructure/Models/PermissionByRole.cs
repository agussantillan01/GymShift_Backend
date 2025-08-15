using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models
{
    public class PermissionByRole
    {
        public int IdRol { get; set; }
        public Role Role { get; set; }
        public int IdPermiso { get; set; }
        public Permission Permiso { get; set; }
    }
}
