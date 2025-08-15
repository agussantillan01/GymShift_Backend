using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models
{
    public class Group : IdentityRole<int>
    {
        public Group() : base() { }
        public Group(string roleName) : base(roleName)
        {

        }
    }
}
