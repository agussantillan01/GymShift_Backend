﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.DTOs.Account
{
    public class AuthenticationRequest
    {
        public string Usuario { get; set; }
        public string Password { get; set; }
    }
}
