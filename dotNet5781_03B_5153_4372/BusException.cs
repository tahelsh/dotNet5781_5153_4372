﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781_03B_5153_4372
{
    public class BusException : Exception
    {

        public BusException() : base() { }
        public BusException(string message) : base(message) { }
        public BusException(string message, Exception inner) : base(message, inner) { }
        protected BusException(SerializationInfo info, StreamingContext context)
         : base(info, context) { }
        //special constructors:

    }
}
