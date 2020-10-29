using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Configuration;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781_01_5153_4372
{
    class Program
    {
        static Random rand;//rundom number of a ride
        static void Main(string[] args)
        {

            List<Bus> buses = new List<Bus>();
            Choice choice;

            do
            {
                Console.WriteLine("Menu:");
                Console.WriteLine("Choose one of the folowing:");
                Console.WriteLine("0: Add a new but to bus-list ");
                Console.WriteLine("1: Choose bus for travel");
                Console.WriteLine("2: Add refuel or treatment");
                Console.WriteLine("3: Show kilomerage since last treatment");
                Console.WriteLine("-1: Exit");
                bool success = Enum.TryParse(Console.ReadLine(), out choice);
                while (success == false)
                {
                    Console.WriteLine("enter your choice again");
                    success = Enum.TryParse(Console.ReadLine(), out choice);
                }
                switch (choice)
                {
                    case Choice.ADD_BUS:
                        {
                            InsertBus(buses);
                            break;
                        }
                    case Choice.PICK_BUS:
                        {
                            Pick(buses);
                            break;
                        }
                }

            } while (choice != Choice.EXIT);





        }
        public static void InsertBus(List<Bus> buses)//the function adds a new bus to the buses list
        {
            Console.WriteLine("Enter liscense number and date of start operation");
            String lisNum = Console.ReadLine();
            DateTime date;//the final date
            bool success = DateTime.TryParse(Console.ReadLine(), out date);
            if (!success)//checking if the date is valid. otherwise print ERROR
            {
                Console.WriteLine("Error, invalid date");
                return;
            }
            try
            {
                Bus b = new Bus { LisNum = lisNum, dateStart = date, TotalKm=0, fuel=0 };//assuming a new bus's kilometerage and fuel are 0;
                buses.Add(b);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public static void Pick(List<Bus> buses)
        {
            int num;
            Console.WriteLine("Enter liscense number:");
            String lisNum = Console.ReadLine();
            // input validation check
            bool b = int.TryParse(lisNum, out num);
            if (!b)
            {
                Console.WriteLine("Liscense number is invalid");
                return;
            }
            bool exist = false;
            foreach (Bus bus in buses)
            {
                if (bus.LisNum == lisNum)
                {
                    exist = true;
                    int km = rand.Next(1, 20000);
                    if (bus.CanTravel(km) == true)
                    {
                        bus.TotalKm += km;
                        bus.fuel -= km;
                        return;
                    }
                    else
                        Console.WriteLine("The bus can not go for the ride");
                    return;

                }
            }
            if(!exist)
            {
                Console.WriteLine("ERROR, the is no bus with the liscense number entered");
            }
        }
       
            
    }
}
