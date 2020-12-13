using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace dotNet5781_03B_5153_4372
{
    /// <summary>
    /// Interaction logic for DataRefuelTread.xaml
    /// </summary>
    public partial class DataTread : Window
    {
        public ProgressBar ProgressBar { get; set; }
        public Label Label { get; set; }

        public int Seconds { get; set; }
        public Bus Bus { get; set; }

        public DataTread(ProgressBar pb, Label l, int sec, Bus b)
        {
            InitializeComponent();
            ProgressBar = pb;
            Label = l;
            Seconds = sec;
            Bus = b;
        }
        public DataTread(int sec, Bus b)
        {
            Seconds = sec;
            Bus = b;
        }
    }
}
