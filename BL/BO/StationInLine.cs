using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class StationInLine
    {
        public int StationCode { get; set; }
        public int LineStationIndex { get; set; }
        public string Name { get; set; }
        public bool DisabledAccess { get; set; }

    }
}
