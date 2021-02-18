﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class AdjacentStation
    {
        public int StationCode1 { get; set; }
        public int StationCode2 { get; set; }
        public double Distance { get; set; }//the distance between the stations
        public TimeSpan Time { get; set; }//the travel time between the stations
        //public bool IsDeleted { get; set; }
    }
}
