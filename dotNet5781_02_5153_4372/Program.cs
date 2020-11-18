using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace dotNet5781_02_5153_4372
{
    class Program
    {
        static void Main(string[] args)
        {
            List<BusStation> stations = new List<BusStation>();
            for(int i=0; i<40; i++)
            {
                stations.Add(new BusStation());
            }
            List<BusLine> lines = new List<BusLine>();
            LineCollection linesCollection = new LineCollection(lines);
            //intialization of both lists.
            Menu choice;
            do
            {
                Console.WriteLine("Choose one of the following");
                Console.WriteLine("0: add");
                Console.WriteLine("1: delete");
                Console.WriteLine("2: search");
                Console.WriteLine("3: print");
                Console.WriteLine("4: exit");
                char ch;
                while (!Menu.TryParse(Console.ReadLine(), out choice))
                    Console.WriteLine("ERROR, enter your choice again");
                switch (choice)
                {
                    case Menu.ADD:
                        Console.WriteLine("enter a for adding a new bus line");
                        Console.WriteLine("enter b for adding a new station to an existing bus line");
                        ch = Console.ReadLine()[0];
                        try
                        {
                            if (ch == 'a')
                            {
                                AddNewBus(linesCollection, stations);
                                break;
                            }
                            if (ch == 'b')
                            {
                                AddNewStation(linesCollection, stations);
                                break;
                            }
                            else
                                Console.WriteLine("ERROR, invalid choice");

                        }
                        catch (FormatException) { Console.WriteLine("the value that entered invalid"); }
                        catch (BusLineException ex) { Console.WriteLine(ex.Message); }
                        catch (BusStationException ex) { Console.WriteLine(ex.Message); }
                        break;
                    case Menu.DELETE:
                        Console.WriteLine("enter a for deleting a bus line");
                        Console.WriteLine("enter b for deleting a bus station of an existing bus line");
                        ch = Console.ReadLine()[0];
                        try
                        {
                            if (ch == 'a')
                                DeleteBus(linesCollection);
                            if (ch == 'b')
                                RemoveStation(linesCollection);
                        }
                        catch (BusLineException ex) { Console.WriteLine(ex.Message); }
                        catch (FormatException) { Console.WriteLine("the value that entered invalid"); }
                        break;
                    case Menu.SEARCH:
                        Console.WriteLine("enter a for searching a bus lines in a station");
                        Console.WriteLine("enter b for searching buses matching a route");
                        ch = Console.ReadLine()[0];
                        try
                        {
                            if (ch == 'a')
                                LinesInStation(linesCollection);
                            else if (ch == 'b')
                                BusesInRoute(linesCollection);
                            else
                                Console.WriteLine("ERROR, invalid choice");

                        }
                        catch (FormatException) { Console.WriteLine("the value that entered invalid"); }
                        catch (BusStationException ex) { Console.WriteLine(ex.Message); }
                        break;
                    case Menu.PRINT:
                        Console.WriteLine("enter a for printing all the bus lines");
                        Console.WriteLine("enter b for printing a list of all stations and line numbers passing through them");
                        ch = Console.ReadLine()[0];
                        if (ch == 'a')
                            PrintAllLines(linesCollection);
                        if (ch == 'b')
                            PrintListStations(stations, linesCollection);
                        break;
                    case Menu.EXIT:
                        Console.WriteLine("Bye");
                        break;
                }
            } while (choice != Menu.EXIT);
        }
        public static void AddNewBus(LineCollection collection, List<BusStation> stations)
        {
            Console.WriteLine("enter the line number");
            int lineNum = int.Parse(Console.ReadLine());
            Console.WriteLine("enter the codes of stations, for ending enter -1");
            int code = int.Parse(Console.ReadLine());
            List<BusLineStation> route = new List<BusLineStation>();
            while (code != -1)
            {
                int index = IndexOfStation(stations, code);
                if (index == -1)
                    throw new BusStationException("The station does not exist");
                TimeSpan time = new TimeSpan(0, 0, 0);
                if (route.Count != 0)
                {
                    Console.WriteLine("enter travel time from the previous station.");
                    while (!TimeSpan.TryParse(Console.ReadLine(), out time))
                    {
                        Console.WriteLine("ERROR, enter time again");
                    }
                }
                BusStation stopTmp = stations[index];
                //BusLineStation stop = new BusLineStation(stopTmp.Code, time, stopTmp.Adress) {Latitude = stopTmp.Latitude, Longitude = stopTmp.Longitude};
                BusLineStation stop = new BusLineStation(stopTmp, time);
                if (route.Count == 0)
                    stop.Distance = 0;
                route.Add(stop);
                Console.WriteLine("enter the codes of stations, for ending enter -1");
                code = int.Parse(Console.ReadLine());
            }
            Console.WriteLine("enter the area of the bus line's route");
            Console.WriteLine("1:general 2:North 3:South 4:Center 5:Jerusalem ");
            Areas area;
            while(!Areas.TryParse(Console.ReadLine(),out area))
            {
                Console.WriteLine("ERROR, enter the area again");
            }
            collection.AddLine(new BusLine(lineNum, route, area));
        }
        public static int IndexOfStation(List<BusStation> list, int code)
        {
            int index = 0;
            foreach (BusStation s in list)
            {
                if (s.Code == code)
                    return index;
                index++;
            }
            return -1;
        }
        public static void AddNewStation(LineCollection collection, List<BusStation> stations)
        {
            Console.WriteLine("enter line number, first station and last station.");
            int lineNum = int.Parse(Console.ReadLine());
            int firstStation = int.Parse(Console.ReadLine());
            int lastStation = int.Parse(Console.ReadLine());
            foreach (BusLine b in collection)
            {
                if (b.LineNum == lineNum && b.FirstStation.Code == firstStation && b.LastStation.Code == lastStation)
                {
                    Console.WriteLine("enter the code of new station");
                    int code = int.Parse(Console.ReadLine());
                    int index = IndexOfStation(stations, code);
                    if (index == -1)
                        throw new BusStationException("the station does not exist");
                    if (b.Exist(code))
                        throw new BusLineException("the station is already exists in this bus line");
                    Console.WriteLine("enter 0: for adding in the begining, 1: for adding in middle, 2: for adding in the end");
                    Insert c;
                    while (!Insert.TryParse(Console.ReadLine(), out c))
                    {
                        Console.WriteLine("ERROR, enter the choice again");
                    }
                    TimeSpan time = new TimeSpan(0,0,0);
                    if (c != Insert.FIRST)//if the stop is in the begginig so the time fro the previous stop is 0
                    {
                        Console.WriteLine("enter travel time from previous station");
                        time = TimeSpan.Parse(Console.ReadLine());
                    }
                    BusStation stopTmp = stations[index];
                    //BusLineStation stop = new BusLineStation(stopTmp.Code, time, stopTmp.Adress) { Latitude = stopTmp.Latitude, Longitude = stopTmp.Longitude };
                    BusLineStation stop = new BusLineStation(stopTmp, time);
                    b.AddStation(stop, c);
                    return;
                }
            }
            throw new BusLineException("the bus line does not exist");

        }
        public static void DeleteBus(LineCollection collection)
        {
            Console.WriteLine("enter line number, first station and last station of the bus line you want to delete.");
            int lineNum = int.Parse(Console.ReadLine());
            int firstStation = int.Parse(Console.ReadLine());
            int lastStation = int.Parse(Console.ReadLine());
            foreach (BusLine b in collection)
            {
                if (b.LineNum == lineNum && b.FirstStation.Code == firstStation && b.LastStation.Code == lastStation)
                {
                    collection.RemoveBus(b);
                    return;
                }
            }
            throw new BusLineException("the bus line does not exist");
        }
        public static void RemoveStation(LineCollection collection)
        {
            Console.WriteLine("enter line number, first station and last station of the bus line.");
            int lineNum = int.Parse(Console.ReadLine());
            int firstStation = int.Parse(Console.ReadLine());
            int lastStation = int.Parse(Console.ReadLine());
            foreach (BusLine b in collection)
            {
                if (b.LineNum == lineNum && b.FirstStation.Code == firstStation && b.LastStation.Code == lastStation)
                {
                    Console.WriteLine("enter station code to delete");
                    int code = int.Parse(Console.ReadLine());
                    b.DeleteStation(code);
                    return;
                }
            }
            throw new BusLineException("the bus line does not exist");
        }
        public static void LinesInStation(LineCollection collection)
        {
            Console.WriteLine("enter the code of the station");
            int code = int.Parse(Console.ReadLine());
            List<BusLine> buses = collection.BusesInStation(code);
            foreach (BusLine b in buses)
            {
                Console.WriteLine(b.LineNum);
            }
        }
        public static void BusesInRoute(LineCollection collection)
        {
            Console.WriteLine("enter the code of the source station");
            int code1 = int.Parse(Console.ReadLine());
            Console.WriteLine("enter the code of the destination station");
            int code2 = int.Parse(Console.ReadLine());
            List<BusLine> buses = new List<BusLine>();
            foreach (BusLine b in collection)
            {
                try
                {
                    BusLine busRoute = b.SubRoute(code1, code2);
                    buses.Add(busRoute);
                }
                catch (BusStationException) { }
            }
            if (buses.Count == 0)
                throw new BusLineException("There is no line that matches this route");
            LineCollection sortedByTime = new LineCollection(buses);
            buses = sortedByTime.SortedList();
            foreach (BusLine bus in buses)
            {
                Console.WriteLine(bus.LineNum);
            }
        }
        public static void PrintAllLines(LineCollection collection)
        {
            foreach (BusLine b in collection)
            {
                Console.WriteLine(b);
            }
        }
        public static void PrintListStations(List<BusStation> stations, LineCollection collection)
        {
            foreach (BusStation s in stations)
            {
                Console.WriteLine(s);
                foreach (BusLine b in collection)
                {
                    if (b.Exist(s.Code))
                        Console.WriteLine(b.LineNum);
                }
            }
        }
    }
}
