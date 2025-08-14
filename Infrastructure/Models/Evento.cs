using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models
{
    public class Class
    {
        public int id { get; set; }
        public int idTypeClass { get; set; }
        public DateTime dateFrom { get; set; }
        public DateTime dateTo { get; set; }
        public string schedule { get; set; }
        public string duration { get; set; }
        public string days { get; set; }
        public int idModality { get; set; }
        public decimal price { get; set; }
        public string description { get; set; }
        public int amountMax { get; set; }
        public int amount { get; set; }
        public int idCoach { get; set; }
        public int applicationStatus { get; set; }
    }
}
