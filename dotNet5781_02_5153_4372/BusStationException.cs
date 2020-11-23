//Noa Timsit 209844372
//Tahel Sharon 323125153
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace dotNet5781_02_5153_4372
{
    class BusStationException:Exception
    {
        public BusStationException() : base() { }
        public BusStationException(string message) : base(message) { }
        public BusStationException(string message, Exception inner) : base(message, inner) { }
        protected BusStationException(SerializationInfo info, StreamingContext context)
     : base(info, context) { }
        //special constructors:
    }
}
