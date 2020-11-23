//Noa Timsit 209844372
//Tahel Sharon 323125153
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781_02_5153_4372
{
    class BusLineStation: BusStation, IComparable<BusLineStation>
    {
        static Random rand = new Random();

        private double distance;//distance from previous station
        public double Distance
        {
            get { return distance; }
            set
            {
                if (value >= 0)
                    distance = value;
                else
                    throw new BusStationException("the distance is not valid");//throwing an exception while the value entered for distance isn't valid
            }
        }
        
        private TimeSpan timeTravel;//the travel time from the last station to the current station.
        public TimeSpan TimeTravel
        {
            get { return timeTravel; }
            set { timeTravel = value; }
        }


        public BusLineStation(TimeSpan timeTravel, string adress =" "):base(adress)//constructor
        {
            this.distance = rand.NextDouble()*(500);
            this.timeTravel = timeTravel;
        }
        public BusLineStation(BusStation b, TimeSpan time):base(b)//copy constructor
        {
            this.distance = rand.NextDouble() * (500);
            this.timeTravel = time;
        }
        public int CompareTo(BusLineStation other)//implementation of IComparable interface. comparing between 2 station by their codes
        {
            return this.Code.CompareTo(other.Code);
        }
    }
}
