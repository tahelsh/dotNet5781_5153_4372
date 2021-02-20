using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class StationInLine
    {
        public int StationCode { get; set; }//code of the station
        public int LineStationIndex { get; set; }//the index of the station in the route of the line
        public string Name { get; set; }//name of the station
        public bool DisabledAccess { get; set; }//disable access
        public double Distance { get; set; }//fron the next station
        public TimeSpan Time { get; set; }//from the next station
        public override string ToString()
        {
            return "Station code: " + StationCode + "  Line station index: " + LineStationIndex +"  Name: "+ Name;
        }

    }
}
