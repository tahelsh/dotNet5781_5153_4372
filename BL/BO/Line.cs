using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class Line
    {
       public int LineId { get; set; }//line id
       public int LineNum { get; set; }//line number
       public Area Area { get; set; }//area
       public List<StationInLine> Stations { get; set; }//list of stations
        public List<TimeSpan> DepTimes { get; set; }//departure time 
        public override string ToString()
       {
            return " Line number:    " + LineNum + "    Last Station:    "+ Stations[Stations.Count-1].Name;
       }



    }
}
