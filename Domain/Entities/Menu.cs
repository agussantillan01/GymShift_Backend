using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Menu
    {
        public int Id { get; set; }
        public string Constante { get; set; }
        public string SpGrillaConfig { get; set; }
        public string Descripcion { get; set; }
        public DateTime? FechaAlta { get; set; }
        public DateTime? FechaModif { get; set; }
    }
}
