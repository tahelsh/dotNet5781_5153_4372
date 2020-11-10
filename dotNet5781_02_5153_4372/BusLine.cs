using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781_02_5153_4372
{
    class BusLine
    {
        private int lineBus;
        public int LineBus
        {
            get { return lineBus; }
            set { lineBus = value; }
        }

        private int firstStation;
        public int FirstStation
        {
            get { return firstStation; }
            set { firstStation = value; }
        }

        private int lastStation;
        public int LastStation
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




    }
}
