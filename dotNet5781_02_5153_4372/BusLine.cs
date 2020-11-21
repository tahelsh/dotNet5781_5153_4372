using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace dotNet5781_02_5153_4372
{
    class BusLine : IComparable<BusLine>//inheritting the IComparable interface.
    {
        public List<BusLineStation> stations { get; set; }//list of all the stations
        private int lineNum;//the bus line number
        public int LineNum
        {
            get { return lineNum; }
            set { if (lineNum > 0)
                    lineNum = value;
                else
                    throw new BusLineException("The number of the bus line is invalid");
            }
        }

        private BusLineStation firstStation;
        public BusLineStation FirstStation//the first station of a bus line's route
        {
            get { return firstStation; }
            set { firstStation = value; }
        }

        private BusLineStation lastStation;
        public BusLineStation LastStation//the last station of a busline's route
        {
            get { return lastStation; }
            set { lastStation = value; }
        }

        private Areas area;
        public Areas Area//the bus line route's area
        {
            get { return area; }
            set { area = value; }
        }
        
        public BusLine(int lineNum, List<BusLineStation> stations, Areas area)//constructor
        {
            if (stations.Count < 2)
                throw new BusLineException("There is less than 2 stations");
            if (lineNum < 0)
                throw new BusLineException("The number of the bus line is invalid");
            this.lineNum = lineNum;
            this.area = area;
            this.stations = new List<BusLineStation>(stations);
            this.firstStation = this.stations[0];
            this.lastStation = this.stations[stations.Count - 1];
        }
        public override string ToString()//preparing a station for printing
        {
            string route = "";
            foreach (BusLineStation stop in stations)
            {
                route += ("=>" + stop.Code);
            }
            return "bus line:" + lineNum + " in area:" + area + "\nRoute:" + route;
        }
        
        /// <summary>
        /// searching a station by it's code   
        /// </summary>
        /// <param name="code">the code of the station</param>
        /// <returns>the index of the station in the route list</returns>
        public int Search(int code)//searching a station by it's code
        {
            int index = 0;
            foreach (BusLineStation stop in stations)
            {
                if (stop.Code == code)
                {
                    return index;
                }

                index++;
            }
            return -1;
        }
        
        /// <summary>
        /// adding a station to the route list
        /// </summary>
        /// <param name="other">a station that wants to add</param>
        /// <param name="choice">where to add</param>
        public void AddStation(BusLineStation other, Insert choice)//adding a station to a bus route.
        {
            if (Exist(other.Code))//the new station must exist if the stations list. if it doesn't, throw exception.
                throw new BusLineException("the station is already exists in this bus line");
            if (choice == Insert.MIDDLE)//if the station has to be inserted in the middle of the route
            {
                Console.WriteLine("enter the code of the station before the station you want to add");
                int prevStation;
                if (!int.TryParse(Console.ReadLine(), out prevStation))
                {
                    throw new BusLineException("ERROR, invalid input");
                }
                int index = Search(prevStation);
                if (index == -1)//if the station couldn't be found, index=-1
                {
                    throw new BusLineException("the previous station entered doesn't exist");
                }
                Console.WriteLine("enter distance from the station you want to add to the next one.");
                double temp;//the distance from the station has to be added to the next one.
                while (!double.TryParse(Console.ReadLine(), out temp))
                {
                    Console.WriteLine("ERROR, enter distance again");
                }
                Console.WriteLine("enter travel time from the station you want to add to the next one.");
                TimeSpan time;//the travel time fron the station has to be added to the next one.
                while (!TimeSpan.TryParse(Console.ReadLine(), out time))
                {
                    Console.WriteLine("ERROR, enter time again");
                }               
                stations.Insert(++index, other);
                stations[++index].Distance = temp;
                stations[index].TimeTravel = time;
                return;

            }
            if (choice == Insert.LAST)//if the station has to be added at the end of the route
            {
                stations.Add(other);
                lastStation = other;
                return;
            }
            else//if the station has to be added in the beginning of the route.
            {
                Console.WriteLine("enter distance from the station you want to add to the next one.");
                double temp;
                while (!double.TryParse(Console.ReadLine(), out temp))
                {
                    Console.WriteLine("ERROR, enter distance again");
                }
                Console.WriteLine("enter travel time from the station you want to add to the next one.");
                TimeSpan time;
                while (!TimeSpan.TryParse(Console.ReadLine(), out time))
                {
                    Console.WriteLine("ERROR, enter time again");
                }
                stations.Insert(0, other);
                firstStation = other;
                stations[0].Distance = 0;//the distance of the first stop is 0
                stations[1].Distance = temp;
                stations[1].TimeTravel = time;
                return;
            }

        }
        
        /// <summary>
        /// deleting a station from the route list
        /// </summary>
        /// <param name="code">the station code</param>
        public void DeleteStation(int code)
        {
            if (stations.Count <= 2)
                throw new BusLineException("There is no option to delete the station because there is only 2 stations on this bus line");
            int index = Search(code);
            if (index == -1)
            {
                throw new BusLineException("the previous station entered doesn't exist");
            }
            if (index == stations.Count - 1)
            {
                lastStation = stations[stations.Count - 2];
            }
            else if (index == 0)
            {
                firstStation = stations[1];
                stations[1].TimeTravel = new TimeSpan(0,0,0);
                stations[1].Distance = 0;
            }
            else
            {
                Console.WriteLine("enter the time travel from the previous station of the station after that you want to delete");
                TimeSpan time = TimeSpan.Parse(Console.ReadLine());
                Console.WriteLine("enter the distance from the previous station of the station after that you want to delete");
                double distance= double.Parse(Console.ReadLine());
                stations[index + 1].Distance = distance;
                stations[index + 1].TimeTravel = time;
            }
            stations.RemoveAt(index);//removing the station in the place 'index' from the stations list
        }

        /// <summary>
        /// checking if a station exists in the route list
        /// </summary>
        /// <param name="code"></param>
        /// <returns>if the station exists in the route list</returns>
        public bool Exist(int code)
        {
            foreach (BusLineStation s in stations)
            {
                if (s.Code == code)
                    return true;
            }
            return false;
        }
        
        /// <summary>
        /// calculating distance between two stations in the route
        /// </summary>
        /// <param name="b1">the first station</param>
        /// <param name="b2">the second station</param>
        /// <returns>the distance</returns>
        public double Distance(BusLineStation b1, BusLineStation b2)
        {
            if (!(Exist(b1.Code) && Exist(b2.Code)))//if one of the stations or both dont exist in the stations list, throw exception
            {
                throw new BusLineException("ERROR, one or more of the stations entered don't exist in the bus line");
            }
            double distance = 0;
            int index1 = Search(b1.Code);
            int index2 = Search(b2.Code);
            for (int i = ++index1; i <= index2; i++)
            {
                distance += stations[i].Distance;
            }
            return distance;
        }

        /// <summary>
        /// calculating travel time between two stations in the route
        /// </summary>
        /// <param name="b1">the first station</param>
        /// <param name="b2">the second station</param>
        /// <returns>the travel time </returns>
        public TimeSpan TravelTime(BusLineStation b1, BusLineStation b2)//the function gets 2 stations, and returns the travel time between them.
        {
            if (!(Exist(b1.Code) && Exist(b2.Code)))
            {
                throw new BusLineException("ERROR, one or more of the stations entered don't exist in the bus line");
            }
            TimeSpan time = new TimeSpan(0, 0, 0);
            int index1 = Search(b1.Code);
            int index2 = Search(b2.Code);
            for (int i = ++index1; i <= index2; i++)
            {
                time += stations[i].TimeTravel;
            }
            return time;
        }

        /// <summary>
        /// calculating a sub route between two stations
        /// </summary>
        /// <param name="stop1">the first station</param>
        /// <param name="stop2">the second station</param>
        /// <returns>bus line with the sub route</returns>
        public BusLine SubRoute(int stop1, int stop2)//creating a sub route presented by a bus line
        {
            if (!(Exist(stop1) && Exist(stop2)))
            {
                throw new BusLineException("ERROR, one or more of the stations entered don't exist in the bus line");
            }
            List<BusLineStation> route = new List<BusLineStation>();
            int index1 = Search(stop1);
            int index2 = Search(stop2);
            if(index1 >= index2)
            {
                throw new BusStationException("there is no route");
            }
            for (int i = index1; i <= index2; i++)
            {
                route.Add(stations[i]);
            }
            return new BusLine(LineNum, route, area);
        }
        
        public int CompareTo(BusLine other)//the function compares between two bus lines
        {
            TimeSpan t1 = TravelTime(this.FirstStation, this.LastStation);
            TimeSpan t2 = other.TravelTime(other.FirstStation, other.LastStation);
            return t1.CompareTo(t2);
        }

    }
}
