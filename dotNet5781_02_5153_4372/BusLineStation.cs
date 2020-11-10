using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781_02_5153_4372
{
    class BusLineStation: BusStation
    {
        static Random rand = new Random();

        private double distance;
        public double Distance
        {
            get { return distance; }
            set { distance = value; }
        }
        
        private double timeTravel;
        public TimeSpan TimeTravel
        {
            get { return timeTravel; }
            set { timeTravel = value; }
        }

        public BusLineStation(string adress =" ", TimeSpan timeTravel):base(adress)
        {
            this.distance = rand.NextDouble();
            this.timeTravel = timeTravel;
        }
        

    }
}
