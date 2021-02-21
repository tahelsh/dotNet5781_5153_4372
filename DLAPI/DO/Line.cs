using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    public class Line
    {
        public int LineId { get; set; }//line id
        public int LineNum { get; set; }//line number
        public Area Area { get; set; }//area
        public int FirstStation { get; set; }//code of first station
        public int LastStation { get; set; }//code of last station
        public bool IsDeleted { get; set; }
    }
}
