using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models
{
    public class ServicioEmail
    {
        public int Id { get; set; }
        public string DescripcionEmail { get; set; }
        public string EmailEmisor { get; set; }
        public string EmailReceptor { get; set; }
        public string Asunto { get; set; }
        public string Cuerpo { get; set; }
        public DateTime FechaEnvio { get; set; }
    }
}
