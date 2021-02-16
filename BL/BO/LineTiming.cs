using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class LineTiming
    {
        public int LineId { get; set; }
        public int LineNum { get; set; }
        //public TimeSpan EstimatedTime { get; set; }
        //public int SourceCode { get; set; }
        //public int DestinationCode { get; set; }
        public string DestinationStation { get; set; }
        public string Stringtimes { get; set; }
        public override string ToString()
        {
            return LineId + "   " + LineNum  + "   " + DestinationStation + "   " + Stringtimes;
        }

        //public int LineId { get; set; }
        //public int LineNum { get; set; }
        //public TimeSpan EstimatedTime { get; set; }
        //public int SourceCode { get; set; }
        //public int DestinationCode { get; set; }
        //public override string ToString()
        //{
        //    return LineId +
        //  "   " + LineNum + "   " + EstimatedTime + "   " + SourceCode + "   " + DestinationCode;
        //}
    }
}
