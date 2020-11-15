﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781_02_5153_4372
{
    class BusLineStation: BusStation, IComparable<BusLineStation>
    {
        static Random rand = new Random();

        private double distance;
        public double Distance
        {
            get { return distance; }
            set
            {
                if (value > 0)
                    distance = value;
                else
                    throw new BusStationException("the distance is not valid");
            }
        }
        
        private TimeSpan timeTravel;
        public TimeSpan TimeTravel
        {
            get { return timeTravel; }
            set { timeTravel = value; }
        }

        public BusLineStation(TimeSpan timeTravel, string adress =" "):base(adress)
        {
            this.distance = rand.NextDouble()*(500);
            this.timeTravel = timeTravel;
        }
        public BusLineStation(int code, TimeSpan timeTravel, string adress = " ") : base(code,adress)
        {
            this.distance = rand.NextDouble() * (500);
            this.timeTravel = timeTravel;
        }
        public int CompareTo(BusLineStation other)
        {
            return this.Code.CompareTo(other.Code);
        }
        

    }
}