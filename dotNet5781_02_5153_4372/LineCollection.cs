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
        public LineCollection(List<BusLine> list)
        {
            Lines = new List<BusLine>(list);

        }

        public IEnumerator GetEnumerator()//IEnumerable
        {
            return Lines.GetEnumerator();
        }

        public void AddLine(BusLine bus)//add bus line to the collection
        {
            if (IsExist(bus.stations))//if there is a route like the bus in the collection
                throw new BusLineException("there is already bus line with this route");
            int counter = Counter(bus);//how many buses with this line number there are in the collection
            if (counter == 2)//if there is 2 buses
                throw new BusLineException("This bus line number is already exists twice");
            else //counter == 1/0
            {
                Lines.Add(bus);
                return;
            }
        }

        public bool IsExist(List<BusLineStation> list)
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

        public int Counter(BusLine bus)//return how many buses with the line number of the bus that it got there are in the collection
        {
            int counter = 0;
            foreach(BusLine b in Lines)
            {
                if (b.LineNum == bus.LineNum)
                    counter++;
            }
            return counter;
        }
        public int IndexOfBus(BusLine bus)//return the index of a bus in the collection, if the bus does not exist, the function return -1
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
        public void RemoveBus(BusLine bus)
        {
            int index = IndexOfBus(bus);//the index of the bus in the collection
            if (index == -1)//if the bus does not exist in the collection
                throw new BusLineException("the bus does not exist");
            Lines.RemoveAt(index);//delete the bus
        }
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
