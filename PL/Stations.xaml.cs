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
using BLAPI;

namespace PL
{
    /// <summary>
    /// Interaction logic for Stations.xaml
    /// </summary>
    public partial class Stations : Window
    {
        IBL bl;

        public Stations(IBL _bl)
        {
            InitializeComponent();
            bl = _bl;
            LBStations.DataContext = bl.GetAllStations().ToList();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            System.Windows.Data.CollectionViewSource stationViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("stationViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // stationViewSource.Source = [generic data source]
        }

        private void Station_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            BO.Station station = (sender as ListBox).SelectedItem as BO.Station;
            gridDetailsStation.DataContext = station;
            DGLinesInStation.DataContext = station.Lines;
        }
    }
}
