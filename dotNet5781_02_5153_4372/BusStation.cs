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
        private int code;

        public int Code
        {
            get { return code; }
            set { code = value; }
        }

        private double latitude;

        public double Latitude
        {
            get { return latitude; }
            set { latitude = value; }
        }

        private double longitude;

        public double Longitude
        {
            get { return longitude; }
            set { longitude = value; }
        }

        private string adress;

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
