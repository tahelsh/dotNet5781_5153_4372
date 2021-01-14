using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class LineInStation
    {
        public int LineId { get; set; }
        public int LineNum { get; set; }
        public int LineStationIndex { get; set; }
        public string NameLastStation { get; set; }
        public Area Area { get; set; }
    }
}
