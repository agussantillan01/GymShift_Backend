using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models
{
    public class Permission
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Action { get; set; }
        public string Description { get; set; }
        public string ClaimType { get; set; }
        public ICollection<PermissionByRole> PermissionsByRole { get; set; }
    }
}
