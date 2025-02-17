using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models
{
    public class Permiso
    {
        public int Id { get; set; }
        public string Tipo { get; set; }
        public string Accion { get; set; }
        public string Descripcion { get; set; }
        public string ClaimType { get; set; }
        public ICollection<PermisoXRol> PermisosXRol { get; set; }
    }
}
