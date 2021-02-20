using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class Station
    {
        public int Code { get; set; }//code
        public string Name { get; set; }//name
        public string Address { get; set; }//addresss
        public bool DisabledAccess { get; set; }//disables access
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public List<LineInStation> Lines { get; set; }//list of lines in that pass in this station
        public override string ToString()
        {
            return "Code: "+Code;
        }

    }
}
