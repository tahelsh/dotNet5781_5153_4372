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
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public List<LineInStation> Lines { get; set; }
        public override string ToString()
        {
            return "Code: "+Code;
        }

    }
}
