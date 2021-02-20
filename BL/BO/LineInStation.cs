using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class LineInStation
    {
        public int LineId { get; set; }//line id
        public int LineNum { get; set; }//line number
        public int LineStationIndex { get; set; }//the index of the station in the line
        public string NameLastStation { get; set; }//name of last station
        public Area Area { get; set; }//area od the line
    }
}
