using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.DTOs.Eventos
{
    public class MiEventoView
    {
        public int Id { get; set; }
        public string TipoEvento { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string Horario { get; set; }
        public string Duracion { get; set; }
        public string Dias { get; set; }
        public string Modalidad { get; set; }
        public decimal Valor { get; set; }
        public string Descripcion { get; set; }
        public int CupoMaximo { get; set; }
        public int CupoActual { get; set; }
        public string Profesor { get; set; }
    }
}
