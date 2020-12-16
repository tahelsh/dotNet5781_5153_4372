﻿//Noa Timsit 209844372
//Tahel Sharon 323125153
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace dotNet5781_03B_5153_4372
{
    public class Bus:DependencyObject
    {
        private DateTime dateStart;//date of start operation

        public DateTime DateStart
        {
            get { return dateStart; }
            set { if (value <= DateTime.Now)
                    dateStart = value;
                else
                    throw new BusException("the value for date of start operation is not valid");
            }
        }


        private double totalKm;  //Total Kilometrage

        public double TotalKm
        {
            get { return totalKm;}
            set {if (totalKm <= value)
                    totalKm = value;
                else
                    throw new BusException("the value for total Kilometrage is not valid");// "Error Total Kilometerage"

                 }
        }

        private double fuel;//Fuel

        public double Fuel
        {
            get { return fuel; }
            set { if (value >= 0 && value <= 1200)
                    fuel = value;
                else
                    throw new BusException("the value for fuel is not valid");
            }
        }


        private string licNum; //license number

        public string LicNum
        {
            get { return this.OrderLicenseNumber(); }
            set
            {
                int a;
                bool b = int.TryParse(value, out a);
                if (b)
                {
                    if ((value.Length == 7 && dateStart.Year < 2018) || (value.Length == 8 && dateStart.Year >= 2018))
                        licNum = value;
                    else
                        throw new BusException("the license number / the year is not valid");
                }
                else
                    throw new BusException("the license number is not valid");

            }
        }
        
        private DateTime lastTreat;//the date of the last treatment

        public DateTime LastTreat
        {
            get { return lastTreat; }
            set { if(value <= DateTime.Now && value>=dateStart)
                lastTreat = value; 
            else
                throw new BusException("the value of last treatment is not valid");
            }
        }



        private double kmTreat;//The kilometerage when the last treatment was done

        public double KmTreat
        {
            get { return kmTreat; }
            set {if(value>=0 && value<=totalKm) 
                kmTreat = value;
            else
                 throw new BusException("the value of the kilometerage at the last treatment is not valid");
            }
        }

        public Status BusStatus { get; set; }//Availability status of a bus

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="licNum">license number</param>
        /// <param name="date">date of start operation</param>
        /// <param name="totalKm">total kilometerage</param>
        /// <param name="fuel">fuel in bus</param>
        /// <param name="lastTreat">date of last treatment</param>
        /// <param name="kmTreat">the kilometerage the bus had when treatment was done</param>
        public Bus(string licNum, DateTime date, double totalKm, double fuel, DateTime lastTreat, double kmTreat)
        {
            this.DateStart = date;
            this.LicNum = licNum;
            this.LastTreat = lastTreat;
            if (totalKm < 0)
                throw new BusException("the total kilometrage is not valid");
            this.totalKm = totalKm;
            this.Fuel = fuel;
            this.KmTreat = kmTreat;
            this.BusStatus = Status.Available;  

        }/// <summary>
        /// the function arranges the license number digit the right format
        /// </summary>
        /// <returns>returns the license number in the right format</returns>
        public string OrderLicenseNumber()
        {
            string prefix, middle, suffix;
            if (dateStart.Year < 2018)
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
            return finalLicNum;
        }


        /// <summary>
        /// the function prepares the bus object for printing
        /// </summary>
        /// <returns>returns bus object prepared for print</returns>
        public override string ToString()
        {//to string
            return "License Number: " + LicNum + "\nTotal Kilometrage: " + totalKm + "\nDate Start: " + dateStart + "\nFuel: " + fuel + "\nDate of last Treatment: " + lastTreat + "\nKilometrage from last treatment: " + kmTreat + "\nStatus: " + BusStatus;
        }

        /// <summary> 
        /// checks if the bus need treatment
        /// </summary>
        /// <param name="km">kilometers that it need to travel</param>
        /// <returns></returns>
        public Boolean NeedTreatment(double km=0)
        {
            if (((totalKm - kmTreat) + km >= 20000) || ((DateTime.Now - lastTreat).TotalDays >= 365))
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// checks if the bus has to get refueled
        /// </summary>
        /// <param name="km"></param>
        /// <returns>returns true if the bus has to get refueled. otherwise, returns false</returns>
        public Boolean NeedFuel(double km=0)
        {
            if (fuel - km < 0)
            {
                return true ;
            }
            return false;
        }

        /// <summary>
        /// checs if a bus can fo for a ride
        /// </summary>
        /// <param name="km"></param>
        /// <returns>if it can, it returns true. otherwise, false.</returns>
        public bool CanTravel(double km)//the function checks if the bus can go for the ride.
        {
            if(NeedTreatment(km))
            {
                throw new BusException("The bus "+LicNum+" can not start the driving, it needs tratment");
            }
            if(NeedFuel(km))
            {
                throw new BusException("The bus "+LicNum+ " can not start the driving, it is missing fuel");
            }
            return true;
        }
        /// <summary>
        /// the function refuels the bus
        /// </summary>
        public void Refuel()
        {
            fuel = 1200;
        }
        /// <summary>
        /// the function adds a treatment by updating the suitable bus field.
        /// </summary>
        public void Treatment()
        {
            lastTreat = DateTime.Now;
            kmTreat = totalKm;
        }
        /// <summary>
        /// taking the bus for a ride by updating the suitable fields
        /// </summary>
        /// <param name="km">the distance of the ride</param>
        public void DoRide(double km)
        {
            if (CanTravel(km))
            {
                totalKm += km;
                fuel -= km;
            }
        }
        /// <summary>
        /// checks if the bus is busy
        /// </summary>
        /// <returns>if it is busy, it returns true. otherwise, false</returns>
        public Boolean IsBusy()
        {
            return (BusStatus != Status.Available);
        }
    }
  
}
