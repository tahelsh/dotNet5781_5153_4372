using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace dotNet5781_03B_5153_4372
{
     public class DataThread
    {
        public ProgressBar ProgressBar { get; set; }
        public Label Label { get; set; }
        public Button ButtonStartDriving { get; set; }
        public int Seconds { get; set; }
        public Bus Bus { get; set; }
        public double DistanceDriving { get; set; }

        public DataThread(ProgressBar pb, Label l, Button button, int sec, Bus b)
        {
            ProgressBar = pb;
            Label = l;
            ButtonStartDriving = button;
            Seconds = sec;
            Bus = b;
        }

        public DataThread(ProgressBar pb, Label label, Button button, int sec, Bus b, double dis)
        {
            ProgressBar = pb;
            Label = label;
            ButtonStartDriving = button;
            Seconds = sec;
            Bus = b;
            DistanceDriving = dis;
        }


        /// <summary>
        /// update the details that sent to the thread according to a status of a bus
        /// </summary>
        /// <param name="st">status of a bus</param>
        public void UpdateDetails(Status st)
        {
            switch (st)
            {
                case Status.Refueling:
                    ButtonStartDriving.IsEnabled = false;
                    ProgressBar.Visibility = Visibility.Visible;
                    Label.Visibility = Visibility.Visible;
                    ProgressBar.Foreground = Brushes.Yellow;
                    break;
                case Status.InTravel:
                    ButtonStartDriving.IsEnabled = false;
                    ProgressBar.Visibility = Visibility.Visible;
                    Label.Visibility = Visibility.Visible;
                    ProgressBar.Foreground = Brushes.Aqua;
                    break;
                case Status.Treatment:
                    ButtonStartDriving.IsEnabled = false;
                    ProgressBar.Visibility = Visibility.Visible;
                    Label.Visibility = Visibility.Visible;
                    ProgressBar.Foreground = Brushes.DeepPink;
                    break;
                case Status.Available:
                    ButtonStartDriving.IsEnabled = true;
                    ProgressBar.Visibility = Visibility.Hidden;
                    Label.Visibility = Visibility.Hidden;
                    ProgressBar.Value = 0;
                    Label.Content = "time";
                    break;
            }
        }
    }
}
