using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models
{
    public class UserByRole
    {
        public int Id { get; set; }
        public int IdUser { get; set; }
        public int IdRole { get; set; }
    }
}
