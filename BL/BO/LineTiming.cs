using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class LineTiming
    {
        public int LineId { get; set; }//line id
        public int LineNum { get; set; }//line number
        public string DestinationStation { get; set; }//name of last station
        public string Stringtimes { get; set; }//string of the times of the line in the closer hour
        public override string ToString()
        {
            return LineId + "   " + LineNum  + "   " + DestinationStation + "   " + Stringtimes;
        }
    }
}
