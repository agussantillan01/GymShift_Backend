using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models
{
    public class ActividadesXEntrenador
    {
        public int Id { get; set; }
        public int IdUsuario { get; set; }
        public int IdActividad { get; set; }
    }
}
