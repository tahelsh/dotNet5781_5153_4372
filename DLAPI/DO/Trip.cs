using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    public class Trip
    {
        public int TripId { get; set; }
        public string UserName { get; set; }
        public int LineId { get; set; }
        public int GetOnStation { get; set; }
        public TimeSpan GetOnTime { get; set; }
        public int GetOutStation { get; set; }
        public TimeSpan GetOutTime { get; set; }
        public bool IsDeleted { get; set; }
    }
}
