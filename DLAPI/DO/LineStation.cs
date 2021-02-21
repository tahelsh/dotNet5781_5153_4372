using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    public class LineStation
    {
        public int LineId { get; set; }//line id
        public int StationCode { get; set; }//code of station
        public int LineStationIndex { get; set; }//the index of the station in the line
        public int PrevStationCode { get; set; }//previous station, if its the first station it will be 0
        public int NextStationCode { get; set; }//next station, if its the last station it will be 0
        public bool IsDeleted { get; set; }
    }
}
