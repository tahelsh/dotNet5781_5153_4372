﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    public class User
    {
        public  string UserName { get; set; }//unique username
        public string Password { get; set; }
        public bool AdminAccess { get; set; }
    }
}