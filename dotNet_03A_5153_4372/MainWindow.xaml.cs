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
using System.Windows.Navigation;
using System.Windows.Shapes;
using dotNet5781_02_5153_4372;

namespace dotNet_03A_5153_4372
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private BusLine currentDisplayBusLine;
        private LineCollection busLines;

        public MainWindow()
        {
            InitializeComponent();
            List<BusStation> stations = new List<BusStation>();
            List<BusLine> lines = new List<BusLine>();
            busLines = new LineCollection(lines);
            //intialization of both lists.
            BuildStationsAndBuses.createStationAndBusesLists(ref stations, busLines);
            cbBusLines.ItemsSource = busLines;
            cbBusLines.DisplayMemberPath = "LineNum";
            cbBusLines.SelectedIndex = 0;

        }

        private void cbBusLines_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowBusLine((cbBusLines.SelectedValue as BusLine).LineNum);
        }
        private void ShowBusLine(int index)
        {
            currentDisplayBusLine = busLines[index].First();
            UpGrid.DataContext = currentDisplayBusLine;
            lbBusLineStations.DataContext = currentDisplayBusLine.stations;
        }

    }
}
