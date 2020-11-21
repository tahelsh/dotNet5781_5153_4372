using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace dotNet5781_02_5153_4372
{
    class LineCollection: IEnumerable
    {
       
        public List<BusLine> Lines { get; set; }//list of all the line buses
        
        public LineCollection(List<BusLine> list)//constructor
        {
            Lines = new List<BusLine>(list);

        }

        public IEnumerator GetEnumerator()//IEnumerable
        {
            return Lines.GetEnumerator();
        }

        /// <summary>
        /// adding a bus line to the collection
        /// </summary>
        /// <param name="bus">a bus</param>
        public void AddLine(BusLine bus)
        {
            if (IsRouteExist(bus.stations))//if there is a route like the bus in the collection
                throw new BusLineException("there is already bus line with this route");
            int counter = Counter(bus);//how many buses with this line number there are in the collection
            if (counter == 2)//if there is 2 buses
                throw new BusLineException("This bus line number is already exists twice");
            else if (counter == 1)//if there is 1 bus line
            {
                int index = IndexOfBus(bus.LineNum);
                if (Lines[index].Area != bus.Area)
                    throw new BusLineException("the travel area of the two bus lines are not the same");
            }
            else //counter=0
            {
                Lines.Add(bus);
                return;
            }
        }
        
        /// <summary>
        /// checking if there is a bus with a specific route
        /// </summary>
        /// <param name="list">a route</param>
        /// <returns>if there is a bus with this route</returns>
        public bool IsRouteExist(List<BusLineStation> list)
        {
            foreach (BusLine b in Lines)
            {
                int i;
                for(i=0; i<list.Count && i<b.stations.Count; i++)
                {
                    if (list[i].Code != b.stations[i].Code)
                        break;
                }
                if ((i == list.Count) || (i == b.stations.Count))//if there is a route like the route that the function got
                    return true;
            }
            return false;
        }

        /// <summary>
        /// return how many buses with the line number of the bus that it got there are in the collection
        /// </summary>
        /// <param name="bus">a bus line</param>
        /// <returns></returns>
        private int Counter(BusLine bus) 
        {
            int counter = 0;
            foreach(BusLine b in Lines)
            {
                if (b.LineNum == bus.LineNum)
                    counter++;
            }
            return counter;
        }

        /// <summary>
        ///return the index of a bus in the collection, if the bus does not exist, the function return -1
        /// </summary>
        /// <param name="bus">a bus line</param>
        /// <returns></returns>
        private int IndexOfBus(BusLine bus)
        {
            int index = 0;
            foreach (BusLine b in Lines)
            {
                if (b.LineNum== bus.LineNum && b.FirstStation==bus.FirstStation && b.LastStation== bus.LastStation)
                    return index;
                 index++;
            }
            return -1;
        }

        /// <summary>
        /// return the index of a bus in the collection, if the bus does not exist, the function return -1
        /// </summary>
        /// <param name="numLine">a line number a bus</param>
        /// <returns></returns>
        private int IndexOfBus(int numLine)
        {
            int index = 0;//the index of the bus in the line
            foreach (BusLine b in Lines)
            {
                if (b.LineNum == numLine)
                    return index;
                index++;
            }
            return -1;//if the bus does not exist in the list of lines
        }
        
        /// <summary>
        /// romove a bus line from the collection
        /// </summary>
        /// <param name="bus"> a bus line</param>
        public void RemoveBus(BusLine bus)
        {
            int index = IndexOfBus(bus);//the index of the bus in the collection
            if (index == -1)//if the bus does not exist in the collection
                throw new BusLineException("the bus does not exist");
            Lines.RemoveAt(index);//delete the bus
        }
        
        /// <summary>
        /// return a list of all the bus lines that pass on a station
        /// </summary>
        /// <param name="stationCode">a code of a station</param>
        /// <returns></returns>
        public List<BusLine> BusesInStation(int stationCode)
        {
            List<BusLine> lst = new List<BusLine>();//A list of buses that pass the atation
            foreach(BusLine b in Lines)
            {
                if (b.Exist(stationCode))//if the station exists in the route of a bus
                    lst.Add(b);
            }
            if (lst.Count == 0)//if there are no buses in this stations
                throw new BusStationException("there is no bus that stops in this station.");
            return lst;
        }
        
        /// <summary>
        /// return a sorted list of all the buses in the collection
        /// </summary>
        /// <returns></returns>
        public List<BusLine> SortedList()
        {
            BusLine[] busLineArr = new BusLine[Lines.Count];
            Lines.CopyTo(busLineArr);//copy the buses to an arrry
            List<BusLine> sortedlist = busLineArr.ToList();//copy the array to a list
            sortedlist.Sort();//sort the list bu the travel time
            return sortedlist;//return the sorted list
        }
        public List<BusLine> this[int lineNum]
        {
            get 
            {
                List<BusLine> bsl = Lines.FindAll(item => item.LineNum == lineNum);//list of all the buses with the line number that the function got
                if (bsl.Count!= 0)//if there are buses with this line number
                    return bsl;
                else//if there are not buses with this line number
                    throw new BusLineException("There is no buses with this line number");
            
            
            }
            //set { Lines[lineNum] = value; }

        }

    }
}
