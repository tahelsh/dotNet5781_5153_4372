using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class Line
    {
       public int LineId { get; set; }
       public int LineNum { get; set; }
       public Area Area { get; set; }
       public IEnumerable<LineStation> Stations { get; set; }
       public bool IsDeleted { get; set; }

    }
}
