﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class Station
    {
        public int Code { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public bool DisabledAccess { get; set; }
        public List<LineInStation> Lines { get; set; }
       
    }
}
