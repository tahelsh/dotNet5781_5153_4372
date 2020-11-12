using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace dotNet5781_02_5153_4372
{
    class BusLine : IComparable<BusLine>
    {
        private List<BusLineStation> stations = new List<BusLineStation>();
        private int lineNum;
        public int LineNum
        {
            get { return lineNum; }
            set { lineNum = value; }
        }

        private BusLineStation firstStation;
        public BusLineStation FirstStation
        {
            get { return firstStation; }
            set { firstStation = value; }
        }

        private BusLineStation lastStation;
        public BusLineStation LastStation
        {
            get { return lastStation; }
            set { lastStation = value; }
        }

        private Areas area;
        public Areas Area
        {
            get { return area; }
            set { area = value; }
        }
        public BusLine(int lineNum, List<BusLineStation> stations, Areas area)
        {
            this.lineNum = lineNum;
            this.area = area;
            this.stations = stations;
            this.firstStation = stations[0];
            this.lastStation = stations[stations.Count - 1];
        }
        public override string ToString()
        {
            string line = "";
            foreach (BusLineStation stop in stations)
            {
                line += (stop.ToString() + "=>");
            }
            return "bus line:" + lineNum + " in area:" + area + "Line:" + line;
        }
        private int Search(int code)
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
        public void AddStation(BusLineStation other, Insert choice)
        {
            if (choice == Insert.MIDDLE)
            {
                Console.WriteLine("enter the code of the station before the station you want to add");
                int prevStation;
                if (!int.TryParse(Console.ReadLine(), out prevStation))
                {
                    throw new BusLineException("ERROR, invalid input");
                }
                int index = Search(prevStation);
                if (index == -1)
                {
                    throw new BusLineException("the previous station entered doesn't exist");
                }
                stations.Insert(++index, other);
                return;

            }
            if (choice == Insert.LAST)
            {
                stations.Add(other);
                lastStation = other;
                return;
            }
            else
            {
                stations.Insert(0, other);
                firstStation = other;
                return;
            }

        }
        public void DeleteStation(int code)//this function gets a station code, looks for the station and deletes it.
        {
            int index = Search(code);
            if (index == -1)
            {
                throw new BusLineException("the previous station entered doesn't exist");
            }
            if (index == 0)
            {
                firstStation = stations[1];
            }
            if (index == stations.Count - 1)
            {
                lastStation = stations[stations.Count - 2];
            }
            stations.RemoveAt(index);

        }
        public bool exist(BusLineStation other)//the function checks if a station
        {
            foreach (BusLineStation s in stations)
            {
                if (s == other)
                    return true;
            }
            return false;
        }
        public double Distance(BusLineStation b1, BusLineStation b2)
        {
            if (!(exist(b1) && exist(b2)))
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
        public TimeSpan travelTime(BusLineStation b1, BusLineStation b2)//the function gets 2 stations, and returns the travel time between them.
        {
            if (!(exist(b1) && exist(b2)))
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
        public BusLine SubRoute(BusLineStation b1, BusLineStation b2)
        {
            if (!(exist(b1) && exist(b2)))
            {
                throw new BusLineException("ERROR, one or more of the stations entered don't exist in the bus line");
            }
            List<BusLineStation> route = new List<BusLineStation>();
            int index1 = Search(b1.Code);
            int index2 = Search(b2.Code);
            for (int i = index1; i <= index2; i++)
            {
                route.Add(stations[i]);
            }
            return new BusLine(LineNum, route, area);
        }
        public int CompareTo(BusLine other)//the function compares between two bus lines
        {
            TimeSpan t1 = travelTime(this.FirstStation, this.LastStation);
            TimeSpan t2 = travelTime(other.FirstStation, other.LastStation);
            return t1.CompareTo(t2);
        }

    }
}
