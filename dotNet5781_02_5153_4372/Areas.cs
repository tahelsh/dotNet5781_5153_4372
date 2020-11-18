using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781_02_5153_4372
{
    public enum Areas//enum for the lines areas. 
    {
        General=1, North, South, Center, Jerusalem
    }
    public enum Insert//enum for adding a station to a route. can be added the beginning, middle, or end of the route.
    {
        FIRST, MIDDLE, LAST
    }
    public enum Menu//the menu of the program
    {
        ADD, DELETE, SEARCH, PRINT, EXIT
    }

}
