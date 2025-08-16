using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.DTOs.Clase
{
    public class ClassParemeterDTO
    {
        public int? Id { get; set; }
        public string modality { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public string Hour { get; set; }
        public string Duration { get; set; }
        public string Activity { get; set; }
        public int AmountMax { get; set; }
        public string Description { get; set; } 
        public List<string> Days { get; set; }
        public string Price { get; set; }
    }

}
