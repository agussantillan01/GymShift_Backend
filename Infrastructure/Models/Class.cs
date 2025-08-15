using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models
{
    public class Class
    {
        public int Id { get; set; }
        public int idTypeClass { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public string Schedule { get; set; }
        public string Duration { get; set; }
        public string Days { get; set; }
        public int IdModality { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public int AmountMax { get; set; }
        public int Amount { get; set; }
        public int IdCoach { get; set; }
        public int ApplicationStatus { get; set; }
    }
}
