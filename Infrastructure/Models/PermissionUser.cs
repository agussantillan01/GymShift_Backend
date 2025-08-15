using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models
{
    public class PermissionUser : IdentityUserClaim<int>
    {
        public int IdPermission { get; set; }
    }
}
