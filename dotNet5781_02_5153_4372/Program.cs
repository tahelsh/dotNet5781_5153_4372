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
            List<BusLine> lines = new List<BusLine>();
            LineCollection linesCollection = new LineCollection(lines);
            //intialization of both lists.
            BuildStationsAndBuses.createStationAndBusesLists(ref stations, linesCollection);

            Menu choice;
            do
            {
                Console.WriteLine(@"                                                                                                                    
choose one of the following options:
0: add
1: delete   
2: search
3: print
4: exit");
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
                            if (ch == 'a')//adding a new bus line
                                AddNewBus(linesCollection, stations);
                            else if (ch == 'b')//adding a new station to an existing bus line
                                AddNewStation(linesCollection, stations);
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
                            if (ch == 'a')//deleting a bus line
                                DeleteBus(linesCollection);
                            else if (ch == 'b')//deleting a bus station of an existing bus line
                                RemoveStation(linesCollection);
                            else
                                Console.WriteLine("ERROR, invalid choice");
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
                            if (ch == 'a')//searching a bus lines in a station
                                LinesInStation(linesCollection);
                            else if (ch == 'b')//searching buses matching a route
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
                        if (ch == 'a')//printing all the bus lines
                            PrintAllLines(linesCollection);
                        if (ch == 'b')//printing a list of all stations and line numbers passing through them
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
            int lineNum = int.Parse(Console.ReadLine());//the line number of the bus
            Console.WriteLine("enter the codes of stations, for ending enter -1");
            int code = int.Parse(Console.ReadLine());//the code of the stations of the bus
            List<BusLineStation> route = new List<BusLineStation>();//the route of the bus
            while (code != -1)
            {
                int index = IndexOfStation(stations, code);
                if (index == -1)//if the station does not exist in the stock of the stations
                    throw new BusStationException("The station does not exist");
                TimeSpan time = new TimeSpan(0, 0, 0);//the time from the previous station in the route
                if (route.Count != 0)//if its not the first station
                {
                    Console.WriteLine("enter travel time from the previous station.");
                    while (!TimeSpan.TryParse(Console.ReadLine(), out time))
                    {
                        Console.WriteLine("ERROR, enter time again");
                    }
                }
                BusStation stopTmp = stations[index];//the station from the stock
                BusLineStation stop = new BusLineStation(stopTmp, time);//bulid new bus line station
                if (route.Count == 0)//if its the first station
                    stop.Distance = 0;
                route.Add(stop);//adding the station
                Console.WriteLine("enter the codes of stations, for ending enter -1");
                code = int.Parse(Console.ReadLine());
            }
            Console.WriteLine("enter the area of the bus line's route");
            Console.WriteLine("1:general 2:North 3:South 4:Center 5:Jerusalem ");
            Areas area;//the area of the bus line
            while(!Areas.TryParse(Console.ReadLine(),out area))
            {
                Console.WriteLine("ERROR, enter the area again");
            }
            collection.AddLine(new BusLine(lineNum, route, area));//adding the bus line to the collection
        }
        public static int IndexOfStation(List<BusStation> list, int code)
        {
            int index = 0;//the index of the station
            foreach (BusStation s in list)
            {
                if (s.Code == code)
                    return index;
                index++;
            }
            return -1;//if the stations does not exist in the route
        }
        public static void AddNewStation(LineCollection collection, List<BusStation> stations)
        {
            Console.WriteLine("enter line number, first station and last station.");
            int lineNum = int.Parse(Console.ReadLine());//the line number of the bus
            int firstStation = int.Parse(Console.ReadLine());//the first station of the bus
            int lastStation = int.Parse(Console.ReadLine());//the last station of the bus
            foreach (BusLine b in collection)
            {
                if (b.LineNum == lineNum && b.FirstStation.Code == firstStation && b.LastStation.Code == lastStation)
                {
                    Console.WriteLine("enter the code of new station");
                    int code = int.Parse(Console.ReadLine());//the code od the station
                    int index = IndexOfStation(stations, code);
                    if (index == -1)//if the station does not exist in the stock of the stations
                        throw new BusStationException("the station does not exist");
                    if (b.Exist(code))//if the station is already exist in its route 
                        throw new BusLineException("the station is already exists in this bus line");
                    Console.WriteLine("enter 0: for adding in the begining, 1: for adding in middle, 2: for adding in the end");
                    Insert c;//where the user wants to add the station
                    while (!Insert.TryParse(Console.ReadLine(), out c))
                    {
                        Console.WriteLine("ERROR, enter the choice again");
                    }
                    TimeSpan time = new TimeSpan(0,0,0);//the time of the new station from the previous station
                    if (c != Insert.FIRST)//if the stop is in the begginig so the time fro the previous stop is 0
                    {
                        Console.WriteLine("enter travel time from previous station");
                        time = TimeSpan.Parse(Console.ReadLine());
                    }
                    BusStation stopTmp = stations[index];//the stop
                    BusLineStation stop = new BusLineStation(stopTmp, time);//build a new bus line station
                    b.AddStation(stop, c);//adding the stop to the bus's route
                    return;
                }
            }
            throw new BusLineException("the bus line does not exist");

        }
        public static void DeleteBus(LineCollection collection)
        {
            Console.WriteLine("enter line number, first station and last station of the bus line you want to delete.");
            int lineNum = int.Parse(Console.ReadLine());//the line number of the bus
            int firstStation = int.Parse(Console.ReadLine());//the first station of the bus
            int lastStation = int.Parse(Console.ReadLine());//the last station of the bus
            foreach (BusLine b in collection)
            {
                if (b.LineNum == lineNum && b.FirstStation.Code == firstStation && b.LastStation.Code == lastStation)
                {
                    collection.RemoveBus(b);//remove the bus
                    return;
                }
            }
            throw new BusLineException("the bus line does not exist");
        }
        public static void RemoveStation(LineCollection collection)
        {
            Console.WriteLine("enter line number, first station and last station of the bus line.");
            int lineNum = int.Parse(Console.ReadLine());//the line number of the bus
            int firstStation = int.Parse(Console.ReadLine());//the first station of the bus
            int lastStation = int.Parse(Console.ReadLine());//the last station of the bus
            foreach (BusLine b in collection)
            {
                if (b.LineNum == lineNum && b.FirstStation.Code == firstStation && b.LastStation.Code == lastStation)
                {
                    Console.WriteLine("enter station code to delete");
                    int code = int.Parse(Console.ReadLine());//the code of the station that the user wants to delete
                    b.DeleteStation(code);//delete the station
                    return;
                }
            }
            throw new BusLineException("the bus line does not exist");
        }
        public static void LinesInStation(LineCollection collection)
        {
            Console.WriteLine("enter the code of the station");
            int code = int.Parse(Console.ReadLine());//the code od the station
            List<BusLine> buses = collection.BusesInStation(code);//list of all the buses that pass in this station
            foreach (BusLine b in buses)//prints all the buses in this station
            {
                Console.WriteLine(b.LineNum);
            }
        }
        public static void BusesInRoute(LineCollection collection)
        {
            Console.WriteLine("enter the code of the source station");
            int code1 = int.Parse(Console.ReadLine());//the code of the source station
            Console.WriteLine("enter the code of the destination station");
            int code2 = int.Parse(Console.ReadLine());//the code of the destination station
            List<BusLine> buses = new List<BusLine>();//lis of all the sub routes of the buses that pass in those stations
            foreach (BusLine b in collection)
            {
                try
                {
                    BusLine busRoute = b.SubRoute(code1, code2);
                    buses.Add(busRoute);
                }
                catch (BusStationException) { }//if the stations do not exist in the bus 
            }
            if (buses.Count == 0)//if there is no bus that pass on those stations
                throw new BusLineException("There is no line that matches this route");
            LineCollection sortedByTime = new LineCollection(buses);//sort the buses by their travel time
            buses = sortedByTime.SortedList();
            foreach (BusLine bus in buses)//print the buses
            {
                Console.WriteLine(bus.LineNum);
            }
        }
        public static void PrintAllLines(LineCollection collection)//print all the buses
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
                Console.WriteLine(s);//print each station with all the buses that pass on it
                foreach (BusLine b in collection)
                {
                    if (b.Exist(s.Code))
                        Console.WriteLine(b.LineNum);
                }
            }
        }
    }
}
