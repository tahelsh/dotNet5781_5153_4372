//Noa Timsit 209844372
//Tahel Sharon 323125153
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
        private DateTime dateStart;//date of start operation

        public DateTime DateStart
        {
            get { return dateStart; }
            set { if (value <= DateTime.Now)
                    dateStart = value;
                else
                    throw new Exception("The value for date of start operation is not valid");
            }
        }


        private double totalKm;  //Total Kilometrage

        public double TotalKm
        {
            get { return totalKm;}
            set {if (totalKm <= value)
                    totalKm = value;
                else
                    throw new Exception("The value for total KM is not valid");// "Error Total Kilometerage"

                 }
        }

        private double fuel;//Fuel

        public double Fuel
        {
            get { return fuel; }
            set { if (fuel >= 0 && fuel <= 1200)
                    fuel = value;
                else
                    throw new Exception("The value for fuel is not valid");
            }
        }


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
                    if ((value.Length == 7 && dateStart.Year < 2018) || (value.Length == 8 && dateStart.Year >= 2018))
                        licNum = value;
                    else
                        throw new Exception("The license number / the year is not valid");
                }
                else
                    throw new Exception("The license number is not valid");

            }
        }
        
        private DateTime lastTreat;//the date of the last treatment

        public DateTime LastTreat
        {
            get { return lastTreat; }
            set { if(lastTreat <= DateTime.Now)
                lastTreat = value; 
            else
                throw new Exception("The value of last treatment is not valid");
            }
        }



        private double kmTreat;//The kilometerage when the last treatment was done

        public double KmTreat
        {
            get { return kmTreat; }
            set {if(kmTreat>=0 && kmTreat<=totalKm) 
                kmTreat = value;
            else
                    throw new Exception("The value of the kilometerage at the last treatment is not valid");
            }
        }


        public Bus(string licNum, DateTime date, double totalKm, double fuel, DateTime lastTreat, double kmTreat)
        {//constructor
            this.dateStart = date;
            this.licNum = licNum;
            this.lastTreat = lastTreat;
            this.TotalKm = totalKm;
            this.fuel = fuel;
            this.kmTreat = kmTreat;
        }



        public override string ToString()
        {//to string
            string prefix, middle, suffix;
            if(dateStart.Year<2018)
            {
                prefix = licNum.Substring(0, 2);
                middle = licNum.Substring(2, 3);
                suffix = licNum.Substring(5, 2);
            }
            else
            {
                prefix = licNum.Substring(0, 3);
                middle = licNum.Substring(3, 2);
                suffix = licNum.Substring(5, 3);
            }
            string finalLicNum = String.Format("{0}-{1}-{2}", prefix, middle, suffix);
            return "License Number: " + finalLicNum+" Total KM: "+totalKm;
        }

        public bool CanTravel(int km)//the function checks if the bus can go for the ride.
        {
            if(((totalKm-kmTreat)+km >= 20000) || ((DateTime.Now - lastTreat).TotalDays >= 365))
            {
                Console.WriteLine("The bus cannot go for the ride, it needs tratment");
                return false;
            }
            if(fuel - km < 0)
            {
                Console.WriteLine("The bus cannot go for the ride, it is missing fuel");
                return false;
            }
            return true;
        }
        public void Refuel()//the function refuel the bus
        {
            fuel = 1200;
        }
        public void Treatment()//the function update the suitable fields after a treatment
        {
            lastTreat = DateTime.Now;
            kmTreat = totalKm;
        }
    }
  
}
