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
       
        public List<BusLine> Lines { get; set; }
        public LineCollection(List<BusLine> list)
        {
            Lines = new List<BusLine>(list);

        }

        public IEnumerator GetEnumerator()
        {
            return Lines.GetEnumerator();
        }

        public void AddLine(BusLine bus)
        {
            if (IsExist(bus.stations))
                throw new BusLineException("there is already bus line with this route");
            int counter = Counter(bus);
            if (counter == 2)
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
                if ((i == list.Count) || (i == b.stations.Count))
                    return true;
            }
            return false;
        }

        public int Counter(BusLine bus)
        {
            int counter = 0;
            foreach(BusLine b in Lines)
            {
                if (b.LineNum == bus.LineNum)
                    counter++;
            }
            return counter;
        }
        public int IndexOfBus(BusLine bus)
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

        /*public int GetIndexByLineNum(BusLine bus)
        {
            int index = 0;
            foreach (BusLine b in Lines)
            {
                if (b.LineNum == bus.LineNum)
                    return index;
                index++;
            }
            return -1;
        }
        */
        public void RemoveBus(BusLine bus)
        {
            int index = IndexOfBus(bus);
            if (index == -1)
                throw new BusLineException("the bus does not exist");
            Lines.RemoveAt(index);
        }
        public List<BusLine> BusesInStation(int stationCode)
        {
            List<BusLine> lst = new List<BusLine>();
            foreach(BusLine b in Lines)
            {
                if (b.Exist(stationCode))
                    lst.Add(b);
            }
            if (lst.Count == 0)
                throw new BusStationException("there is no bus that stops in this station.");
            return lst;
        }
        public List<BusLine> SortedList()
        {
            BusLine[] busLineArr = new BusLine[Lines.Count];
            Lines.CopyTo(busLineArr);
            List<BusLine> sortedlist = busLineArr.ToList();
            sortedlist.Sort();
            return sortedlist;
        }
        
        public List<BusLine> this[int lineNum]
        {
            get 
            {
                List<BusLine> bsl = Lines.FindAll(item => item.LineNum == lineNum);
                if (bsl.Count!= 0)
                    return bsl;
                else
                    throw new BusLineException("There is no buses with this line number");
            
            
            }
            //set { Lines[lineNum] = value; }

        }




    }
}
