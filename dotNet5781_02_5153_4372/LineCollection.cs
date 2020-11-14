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
            Lines = list;

        }

        public IEnumerator GetEnumerator()
        {
            return Lines.GetEnumerator();
        }

        public void AddLine(BusLine bus)
        {
            int counter = Counter(bus);
            if (counter == 2)
                throw new BusLineException("This bus is already exists twice");
            if (counter == 1)
            {
                int index = IndexOfBus(bus);
                if (bus.FirstStation == Lines[index].LastStation && bus.LastStation == Lines[index].FirstStation)
                {
                    Lines.Add(bus);
                    return;
                }
                else
                    throw new BusLineException("The first and last stations of the buses don't match");
            }
            else//counter==0
                Lines.Add(bus);

        }

        public int Counter(BusLine bus)
        {
            int counter = 0;
            foreach(BusLine b in Lines)
            {
                if (b.LineNum == bus.LineNum && b.FirstStation == bus.FirstStation && b.LastStation == bus.LastStation)
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
                if (b.Search(stationCode) != -1)
                    lst.Add(b);
            }
            if (lst.Count == 0)
                throw new BusStationException("there is no bus that stops in this station.");
            return lst;
        }
        public List<BusLine> SortedList()
        {
            List<BusLine> lst = new List<BusLine>(Lines);
            lst.Sort();
            return lst;
        }
        //public BusLine this[int lineNum]
        //{
        //    get { return Lines[IndexOfBus(lineNum); }
        //    set { arr[i] = value; }
        
        //}




    }
}
