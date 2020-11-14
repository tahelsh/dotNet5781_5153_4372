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
                if (b == bus)
                    counter++;
            }
            return counter;
        }
        public int IndexOfBus(BusLine bus)
        {
            int index = 0;
            foreach (BusLine b in Lines)
            {
                if (b == bus)
                    return index;
                 index++;
            }
            return -1;
        }



    }
}
