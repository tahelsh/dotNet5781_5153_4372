using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    public class LineTrip
    {
        public int LineId { get; set; }//line id
        public TimeSpan StartAt { get; set; }// departure time
        public bool IsDeleted { get; set; }
    }
}
