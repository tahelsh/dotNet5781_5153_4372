using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781_02_5153_4372
{
    class Program
    {
        static void Main(string[] args)
        {
            List<BusStation> stations = new List<BusStation>();
            List<BusLine> lines = new List<BusLine>();
            LineCollection linesCollection = new LineCollection(lines);
            //intialization of both lists.
            Console.WriteLine("Choose one of the following");
            Console.WriteLine("0: add");
            Console.WriteLine("1: delete");
            Console.WriteLine("2: search");
            Console.WriteLine("3: print");
            Console.WriteLine("4: exit");
            Menu choice;
            while (!Menu.TryParse(Console.ReadLine(), out choice))
                Console.WriteLine("ERROR, enter your choice again");
            switch (choice)
            {
                case Menu.ADD:
                    Console.WriteLine("enter a for adding a new bus line");
                    Console.WriteLine("enter b for adding a new station to an existing bus line");
                    char ch = Console.ReadLine()[0];
                    if(ch=='a')
                    {
                        try { AddNewBus(linesCollection, stations); }
                        catch (FormatException) { }
                    }
                    if (ch == 'b')
                    {
                        try { addNewStation(linesCollection, stations); }
                        catch (FormatException) { Console.WriteLine("the value that entered invalid"); }
                        catch (BusLineException ex) { Console.WriteLine(ex.Message); }
                        catch (BusStationException ex) { Console.WriteLine(ex.Message); }
                    }
                    else
                    {
                        Console.WriteLine("ERROR, invalid choice");
                    }
                    break;
                case Menu.DELETE:
                    break;
                case Menu.SEARCH:
                    break;
                case Menu.PRINT:
                    break;
                case Menu.EXIT:
                    break;
                default:
                    break;
            }
        }
        public static void AddNewBus(LineCollection collection, List<BusStation> stations)
        {
            Console.WriteLine("enter the line number");
            int lineNum= int.Parse(Console.ReadLine());
            Console.WriteLine("enter the codes of stations, for ending enter -1");
            int code= int.Parse(Console.ReadLine());
            List<BusLineStation> route = new List<BusLineStation>();
            while (code!=-1)
            {
                int index = indexOfStation(stations, code);
                if (index == -1)
                    throw new BusStationException("The station does not exist");
                Console.WriteLine("enter travel time from the previous station.");
                TimeSpan time;
                while (!TimeSpan.TryParse(Console.ReadLine(), out time))
                {
                    Console.WriteLine("ERROR, enter time again");
                }
                BusStation b=stations[index];
                BusLineStation stop = new BusLineStation(time) { Code = b.Code, Latitude = b.Latitude, Longitude = b.Longitude, Adress = b.Adress };
                route.Add(stop);
            }
            Console.WriteLine("enter the area of the bus line's route");
            Areas area =(Areas) Enum.Parse(typeof(Areas), Console.ReadLine());
            collection.AddLine(new BusLine(lineNum, route, area));
        }

        public static int indexOfStation(List<BusStation> list, int code)
        {
            int index = 0; 
            foreach(BusStation s in list)
            {
                if (s.Code == code)
                    return index;
                index++;
            }
            return -1;
        }
        public static void addNewStation(LineCollection collection, List<BusStation> stations)
        {
            Console.WriteLine("enter line number, first station and last station.");
            int lineNum = int.Parse(Console.ReadLine());
            int firstStation= int.Parse(Console.ReadLine());
            int lastStation= int.Parse(Console.ReadLine());
            bool flag = false;
            foreach(BusLine b in collection)
            {
                if(b.LineNum==lineNum && b.FirstStation.Code==firstStation && b.LastStation.Code==lastStation)
                {
                    flag = true;
                    Console.WriteLine("enter the code of new station");
                    int code = int.Parse(Console.ReadLine());
                    int index = indexOfStation(stations, code);
                    if (index == -1)
                        throw new BusStationException("the station does not exist");
                    Console.WriteLine("enter travel time from previous station");
                    TimeSpan time = TimeSpan.Parse(Console.ReadLine());
                    BusStation st = stations[index];
                    BusLineStation stop = new BusLineStation(time) { Code = st.Code, Latitude = st.Latitude, Longitude = st.Longitude, Adress = st.Adress };
                    Console.WriteLine("enter 0: for adding in the begining, 1: for adding in middle, 2: for adding in the end");
                    Insert c= (Insert)Enum.Parse(typeof(Insert), Console.ReadLine());
                    b.AddStation(stop, c);
                }
            }
            if(flag==false)
                throw new BusLineException("the bus line does not exist");

        }
    }
}
