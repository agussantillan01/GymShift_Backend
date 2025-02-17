using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models
{
    public class PermisoXRol
    {
        public int IdRol { get; set; }
        public Role Rol { get; set; }
        public int IdPermiso { get; set; }
        public Permiso Permiso { get; set; }
    }
}
