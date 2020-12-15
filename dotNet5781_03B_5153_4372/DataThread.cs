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
        public ProgressBar ProgressBar { get; set; }//progress bar for a thread. 
        public Label Label { get; set; }//the label shows the amount of secends left to complete the progress

        public int Seconds { get; set; }//the amount of seconds left to complete the progress. this field is the content of the label.
        public Bus Bus { get; set; }
        public TextBlock TBTotalKm { get; set; }
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="pb">gets progress bar</param>
        /// <param name="l">label</param>
        /// <param name="sec">seconds</param>
        /// <param name="b">bus</param>
        public DataThread(ProgressBar pb, Label l, int sec, Bus b)
        {
            ProgressBar = pb;
            Label = l;
            Seconds = sec;
            Bus = b;
        }
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="pb">gets progress bar</param>
        /// <param name="label">label</param>
        /// <param name="sec">seconds</param>
        /// <param name="b">bus</param>
        /// <param name="TotalKm">total kilometerage of bus</param>
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
