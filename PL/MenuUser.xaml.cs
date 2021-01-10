using BLAPI;
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

namespace PL
{
    /// <summary>
    /// Interaction logic for MenuUser.xaml
    /// </summary>
    public partial class MenuUser : Window
    {
        IBL bl;
        BO.User user;
        public MenuUser(IBL _bl, BO.User _user)
        {
            InitializeComponent();
            bl = _bl;
            user = _user;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            System.Windows.Data.CollectionViewSource lineViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("lineViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // lineViewSource.Source = [generic data source]
            System.Windows.Data.CollectionViewSource stationViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("stationViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // stationViewSource.Source = [generic data source]
        }

        private void StationButton_Click(object sender, RoutedEventArgs e)
        {
            StationsGrid.Visibility = Visibility.Visible;
            linesGrid.Visibility = Visibility.Hidden;
           LBStations.DataContext = bl.GetAllStations().ToList();
        }

        private void LinesButton_Click(object sender, RoutedEventArgs e)
        {
            linesGrid.Visibility = Visibility.Visible;
            StationsGrid.Visibility = Visibility.Hidden;
            LBLines.DataContext = bl.GetAllLines().ToList();
        }

        private void LBLines_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            BO.Line line = (sender as ListBox).SelectedItem as BO.Line;
            linesDetailsGrid.DataContext = line;
            DGstationsInLine.DataContext = line.Stations;
        }

        private void Stations_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            BO.Station station = (sender as ListBox).SelectedItem as BO.Station;
            stationDetailsGrid.DataContext = station;
            linesInStationDataGrid.DataContext = station.Lines;
        }
    }
}
