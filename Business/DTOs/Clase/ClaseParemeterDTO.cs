using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.DTOs.Clase
{
    public class ClaseParemeterDTO
    {
        public string Modalidad { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string Horario { get; set; }
        public string Duracion { get; set; }
        public int Actividad { get; set; }
        public int CupoMaximo { get; set; }
        public string Descripcion { get; set; } 
        public List<string> Dias { get; set; }
        public string Valor { get; set; }
    }

}
