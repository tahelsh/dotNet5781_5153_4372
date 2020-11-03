//Noa Timsit 209844372
//Tahel Sharon 323125153
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Net.Configuration;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781_01_5153_4372
{
    class Program
    {
        static Random rand= new Random(DateTime.Now.Millisecond);//random number of a ride
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
                    case Choice.MAINTENACE_BUS:
                        {
                            MaintenaceBus(buses);
                            break;
                        }
                    case Choice.SHOW_KM:
                        {
                            int i = 1;
                            foreach (Bus b in buses)
                            {
                                Console.WriteLine("Bus " + i + ": "+ b);
                                i++;
                            }
                            break;
                        }
                    case Choice.EXIT:
                        {
                            Console.WriteLine("Bye");
                            break;
                        }


                }

            } while (choice != Choice.EXIT);





        }
        public static void InsertBus(List<Bus> buses)//the function adds a new bus to the buses list
        {
            try
            {
            Console.WriteLine("Enter license number and date of start operation");
            String licNum = Console.ReadLine();//the lic Number
           
            //input the date of start
            DateTime date;//the final date of start
            bool success = DateTime.TryParse(Console.ReadLine(), out date);
            //checking if the date is valid. otherwise print ERROR
            if ((!success)|| date>DateTime.Now)
            {
                Console.WriteLine("Error, invalid date");
                return;
            }
            if (!CheckLiceNum(buses, licNum, date))//check id the licNumber and the date are sutable
                return;
           
            //input total Kilometerage
            Console.WriteLine("Do you want to enter total Kilometerage- enter 'y' for yes and total km, else 'n'");
             double totalKm = 0;//the total Kilometerage, defult value is 0
                if (Console.ReadLine()[0] == 'y')
                {
                    success = double.TryParse(Console.ReadLine(), out totalKm);
                    while ((!success) || totalKm < 0)
                    {
                        Console.WriteLine("ERROR, enter the totalKm again");
                        success = double.TryParse(Console.ReadLine(), out totalKm);
                    }
                }

            //input fuel
            Console.WriteLine("Do you want to enter fuel- enter 'y' for yes and fuel, else 'n'");
                double fuel = 0;//the fuel, defult value is 0
                if (Console.ReadLine()[0] == 'y')
                {
                    success = double.TryParse(Console.ReadLine(), out fuel);
                    while ((!success) || fuel < 0 || fuel > 1200)
                    {
                        Console.WriteLine("ERROR, enter the fuel again");
                        success = double.TryParse(Console.ReadLine(), out fuel);
                    }
                }

            //input the date of last treatment
            Console.WriteLine("Do you want to enter date of last treatment- enter 'y' for yes and the date, else 'n'");
            DateTime lastTreat = date;//the date of last treatment, defult value is the date of start
            if (Console.ReadLine()[0] == 'y')
            {
                success = DateTime.TryParse(Console.ReadLine(), out lastTreat);
                while((!success) || lastTreat > DateTime.Now || lastTreat < date)//while the date is not valid, enter date again
                {
                    Console.WriteLine("Error, invalid date, enter again");
                    success = DateTime.TryParse(Console.ReadLine(), out lastTreat);
                }
            }

            //input the Kilometrage before the last treatment
            Console.WriteLine("Do you want to enter Kilometrage before the last treatment- enter 'y' for yes and Kilometrage before the last treatment, else 'n'");
                double kmTreat = 0;//the Kilometrage before the last treatment, defult value is 0
                if (Console.ReadLine()[0] == 'y')
                {
                    success = double.TryParse(Console.ReadLine(), out kmTreat);
                    while ((!success) || (kmTreat < 0) || (kmTreat > totalKm))
                    {
                        Console.WriteLine("ERROR, enter the number of kilometers till last treatment again");
                        success = double.TryParse(Console.ReadLine(), out kmTreat);
                    }
                }
                //adding the bus to the list
                Bus b = new Bus (licNum, date, totalKm, fuel, lastTreat, kmTreat);//the new bus
                buses.Add(b);
                Console.WriteLine("succsess");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        
        public static void Pick(List<Bus> buses)
         // the function looks for the bus with the license number it inputed, and checks if it eligible for the ride. 
        // if it does, it updates the bus relevant fields.
        {
            int num;//this variable will hold the license number the tryParse function outputs.
            Console.WriteLine("Enter liscense number:");
            String licNum = Console.ReadLine();
            // input validation check
            bool b = int.TryParse(licNum, out num);//b=true when the license number consist of numbers only. else, b=false.
            if (!b)
            {
                Console.WriteLine("Liscense number is invalid");
                return;
            }
            bool exist = false;//if the bus which is looked for exists in the list, exist=true.
            foreach (Bus bus in buses)
            {
                if (bus.LicNum == licNum && exist==false)
                {
                    exist = true;
                    int km = rand.Next(1, 1201);
                    if (bus.CanTravel(km))
                    {
                        bus.TotalKm += km;
                        bus.Fuel -= km;
                        Console.WriteLine("success");
                        return;
                    }
                    else
                        return;
                }
            }
            if(!exist)//if the bus doesn't exist in the list, output an ERROR messeage.
            {
                Console.WriteLine("ERROR, the is no bus with the license number entered");
            }
        }
       
      
        public static void MaintenaceBus(List<Bus> buses)
        //this function gets list of buses, looks for the bus with the license number it input and adds a treatment or a refuel
        {
            Console.WriteLine("Enter license number");
            String licNum = Console.ReadLine();
            bool exist = false;//if the bus which is looked for exists in the list, exist=true.
            foreach (Bus b in buses)
            {
                if (b.LicNum == licNum && exist == false)
                {
                    exist = true;
                    Console.WriteLine("For refuel enter 'r', for a treatment enter 't'");
                    Char ans = Console.ReadLine()[0];//the choice of the user(r or t).
                    while (ans != 'r' && ans != 't')
                    {
                        Console.WriteLine("ERROR, please enter your choice again");
                        ans = Console.ReadLine()[0];
                    }
                    if (ans == 'r')
                    {
                        b.Refuel();
                        Console.WriteLine("success");
                    }
                    if (ans == 't')
                    {
                        b.Treatment();
                        Console.WriteLine("success");
                    }
                }
            }
            if (exist == false)//if the bus doesn't exist in the list, output an ERROR messeage.
                Console.WriteLine("ERROR, there is no bus with this license number.");
        }

        public static bool CheckLiceNum(List<Bus> buses, string licNum, DateTime dateStart )
        //this function checks validation foe the license number.
        {
            foreach (Bus bus in buses)
            {
                if (bus.LicNum == licNum)
                {
                    throw new Exception("There is a license Number in the list");
                }
            }
            int a;//if the tryParse managed to convert the string it got into an iteger, a will hold the integer. 
            bool b = int.TryParse(licNum, out a);
            if (b)
            {
                if ((licNum.Length == 7 && dateStart.Year < 2018) || (licNum.Length == 8 && dateStart.Year >= 2018))
                    return true;
                else
                {
                    throw new Exception("The license number / the year is not valid");
                }
            }
            else
                throw new Exception("The license number is not valid");
        }
    }
}
