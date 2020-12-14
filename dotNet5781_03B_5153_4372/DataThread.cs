using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace dotNet5781_03B_5153_4372
{
     public class DataThread
    {
        public ProgressBar ProgressBar { get; set; }
        public Label Label { get; set; }

        public int Seconds { get; set; }
        public Bus Bus { get; set; }
        public TextBlock TBTotalKm { get; set; }

        public DataThread(ProgressBar pb, Label l, int sec, Bus b)
        {
            ProgressBar = pb;
            Label = l;
            Seconds = sec;
            Bus = b;
        }

        public DataThread(ProgressBar pb, Label label, int sec, Bus b, TextBlock TotalKm)
        {
            ProgressBar = pb;
            Label = label;
            Seconds = sec;
            Bus = b;
            TBTotalKm = TotalKm;
        }
    }
}
