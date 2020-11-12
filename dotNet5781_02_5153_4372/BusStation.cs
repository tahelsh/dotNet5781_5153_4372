using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;


namespace dotNet5781_02_5153_4372
{
   
    class BusStation
    {
        static Random rand = new Random();
        private static int numCode = 0;
        protected int code;

        public int Code
        {
            get { return code; }
            private set { code = value; }
        }

       protected double latitude;

        public double Latitude
        {
            get { return latitude; }
            set {
                if (value >= 31 && value <= 33.3)
                    latitude = value;
                else
                    throw new BusStationException("The latitude is not valid");
                 }
        }

        protected double longitude;

        public double Longitude
        {
            get { return longitude; }
            set {
                if (value >= 34.3 && value <= 35.5)
                    longitude = value;
                else
                    throw new BusStationException("The longitude is not valid");
                }
        }

        protected string adress;

        public string Adress
        {
            get { return adress; }
            set { adress = value; }
        }

        public BusStation(string adress = " ")
        {
            code = numCode++;
            latitude = rand.NextDouble() * (33.3 - 31) + 31;
            longitude = rand.NextDouble() * (35.5 - 34.3) + 34.3;
            this.adress = adress;
        }
        public override String ToString()
        {

            return "Bus Station Code: "+code+", "+latitude+"°N "+longitude+ "°E";
        }
    }
}
