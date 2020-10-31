using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781_01_5153_4372
{
    class Bus
    {
        public DateTime dateStart { get; set; } //date of start operation
       
        private int totalKm;  //Total Kilometrage

        public int TotalKm
        {
            get { return totalKm; }
            set {if (totalKm < value)
                    totalKm = value;
                else
                    throw new Exception("The value for total KM is not valid");// "Error Total Kilometerage"

                 }
        }
        public int fuel { get; set; } //Fuel

        private string licNum; //license number

        public string LicNum
        {
            get { return licNum; }
            set
            {
                int a;
                bool b = int.TryParse(value, out a);
                if (b)
                {
                    if (value.Length == 7 && dateStart.Year < 2018 || value.Length == 8 && dateStart.Year >= 2018)
                        licNum = value;

                }
                else
                    throw new Exception("The license number / the year is not valid");

            }
        }
        public DateTime lastTreat { get; set; }//the date of the last treatment
        public int kmTreat { get; set; }//The kilometerage at the last treatment
        public bool CanTravel(int km)//the function checks if the bus can go for the ride.
        {
            if((totalKm % 20000 +km < 20000 && fuel-km>0)&&((DateTime.Now-lastTreat).TotalDays>=365))
            {
                return true;
            }
            return false;
        }
        public void Refuel()
        {
            fuel = 1200;
        }
        public void Treatment()
        {
            lastTreat = DateTime.Now;
            kmTreat = totalKm;
        }
    }
  
}
