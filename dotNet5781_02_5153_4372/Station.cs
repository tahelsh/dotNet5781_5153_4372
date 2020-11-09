using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace dotNet5781_02_5153_4372
{
   
    class Station
    {
        static Random random = new Random();
        int code;
        static Random rand = new Random();
        double latitude;
        double longitude;
        string adress;
        public Station(string adress)
        {
            code = random.Next() * (100000 - 0) + 0;
            latitude = rand.NextDouble() * (33.3 - 31) + 31;
            longitude = rand.NextDouble() * (35.5 - 34.3) + 34.3;
            this.adress = adress;
        }
        public Station()
        {
            code = random.Next() * (100000 - 0) + 0;
            latitude = rand.NextDouble() * (33.3 - 31) + 31;
            longitude = rand.NextDouble() * (35.5 - 34.3) + 34.3;
        }
        public override String ToString()
        {

            return "Bus Station Code: "+code+", "+latitude+"°N "+longitude+ "°E";
        }
    }
}
