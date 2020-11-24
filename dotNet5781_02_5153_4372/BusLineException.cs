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
    public class BusLineException:Exception
    {

        public BusLineException() : base() { }
        public BusLineException(string message) : base(message) { }
        public BusLineException(string message, Exception inner) : base(message, inner) { }
        protected BusLineException(SerializationInfo info, StreamingContext context)
     : base(info, context) { }
        //special constructors:



    }
}
