using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class Bus
    {
        public int LicenseNum { get; set; }//license number
        public DateTime FromDate { get; set; }//start date
        public double TotalTrip { get; set; }//total km
        public double FuelRemain { get; set; }//fuel tank
        public BusStatus Status { get; set; }//status
        public DateTime DateLastTreat { get; set; }//data of last treatment
        public double KmLastTreat { get; set; }//kilometers when the bus did its last treatment
        public bool IsDeleted { get; set; }//flag of deleting
        public override string ToString()
        {
            return "License Number: " + LicenseNum + " Total KM: " + TotalTrip;
        }

    }
}
