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
            //לשונית של תחנות
            CBStations.SelectedIndex = 0;
            CBStations.DisplayMemberPath = "Name";
            CBStations.DataContext = bl.GetAllStations();
            //לשונית של קווים
            CBLines.SelectedIndex = 0;
            CBLines.DisplayMemberPath = "LineNum";
            CBLines.DataContext = bl.GetAllLines();
            
        }


        private void CBStations_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BO.Station station = CBStations.SelectedItem as BO.Station;
            StationDetailsGrid.DataContext = station;
            linesInStationDataGrid.DataContext = station.Lines;
            LBYellowSign.DataContext = station.Lines;
            codeTextBlock.Text = station.Code.ToString();
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

        private void CBLines_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BO.Line line = CBLines.SelectedItem as BO.Line;
            LineDetailsGrid.DataContext = line;
            //DGstationsInLine.DataContext = line.Stations;
            stationInLineDataGrid.DataContext = line.Stations;
            LBFrequency.DataContext = line.DepTimes;


        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {

            System.Windows.Data.CollectionViewSource lineViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("lineViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // lineViewSource.Source = [generic data source]
            System.Windows.Data.CollectionViewSource stationViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("stationViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // stationViewSource.Source = [generic data source]
            System.Windows.Data.CollectionViewSource stationInLineViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("stationInLineViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // stationInLineViewSource.Source = [generic data source]
        }

        private void SignOut_Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}

