using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    //מחלקה שתסייע לעשות המרה מביאו לדיאו
    public class LineStation
    {
        public int LineId { get; set; }//line id
        public int StationCode { get; set; } //station code
        public int LineStationIndex { get; set; }//index of the station in the line
    }
}
