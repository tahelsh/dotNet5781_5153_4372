using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public enum BusStatus//status of bus
    {
        Available, InTravel, Refueling, Treatment
    }
    public enum Area//enum for the lines areas. 
    {
        General = 1, North, South, Center, Jerusalem
    }
}
