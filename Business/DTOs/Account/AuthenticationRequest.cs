using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.DTOs.Account
{
    public class AuthenticationRequest
    {
        public string usuario { get; set; }
        public string password { get; set; }
    }
}
