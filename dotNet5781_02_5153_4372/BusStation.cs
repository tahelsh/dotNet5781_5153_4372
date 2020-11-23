//Noa Timsit 209844372
//Tahel Sharon 323125153
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
        private static int counter = 0;
        protected int code;

        public int Code //station code
        {
            get { return code; }
            set {if (code <= counter)
                    code = value;
                else
                    throw new BusStationException("the code of the station is invalid");
            }
        }

       protected double latitude;//latitude of bus station

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

        protected double longitude;//longitude of bus station

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

        protected string adress;//adress of station (optional)

        public string Adress
        {
            get { return adress; }
            set { adress = value; }
        }

        public BusStation(string adress = " ")//constructor for abus station (the adress field is optional)
        {
            code = counter++;
            latitude = rand.NextDouble() * (33.3 - 31) + 31;
            longitude = rand.NextDouble() * (35.5 - 34.3) + 34.3;
            this.adress = adress;
        }
       public BusStation(BusStation b)//copy constructor
        {
            this.Code = b.Code;
            this.latitude = b.Latitude;
            this.longitude = b.longitude;
            this.adress = b.Adress;
        }
        public override String ToString()//preparing an object for print
        {

            return "Bus Station Code: " + code + ", " + latitude + "°N " + longitude + "°E";
        }
    }
}
