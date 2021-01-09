using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        ObservableCollection<BO.Station> stations;
        public Stations(IBL _bl)
        {
            InitializeComponent();
            bl = _bl;
            //List<BO.Station> listStations = bl.GetAllStations().ToList();
            //stations = new ObservableCollection<BO.Station>();
            //foreach (var item in listStations)
            //    stations.Add(item);
            //RefreshAllStationsList();
            stations = new ObservableCollection<BO.Station>(bl.GetAllStations());
            LBStations.DataContext = stations;

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            System.Windows.Data.CollectionViewSource stationViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("stationViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // stationViewSource.Source = [generic data source]
        }

        public void RefreshAllStationsList()
        {
            //stations = new ObservableCollection<BO.Station>(bl.GetAllStations());
            //List<BO.Station> listStations = bl.GetAllStations().ToList();
            //foreach (var item in listStations)
            //    stations.Add(item);
            List<BO.Station> stations = bl.GetAllStations().ToList();
            LBStations.DataContext = stations;
        }
        private void winUpdate_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            RefreshAllStationsList();
        }
        private void Station_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            BO.Station station = (sender as ListBox).SelectedItem as BO.Station;
            gridDetailsStation.DataContext = station;
            DGLinesInStation.DataContext = station.Lines;
        }

        private void AddNewStation_Click(object sender, RoutedEventArgs e)
        {
            AddNewStation win = new AddNewStation(bl);
            win.Closing += winUpdate_Closing;
            win.ShowDialog();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int code = int.Parse(codeTextBlock.Text);
                bl.DeleteStation(code);
                foreach (BO.Station item in stations)
                {
                    if (item.Code == code)
                        stations.Remove(item);
                }
                //RefreshAllStationsList();
                MessageBox.Show("station was deleted successfully", "", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (BO.BadStationCodeException ex)
            {
                MessageBox.Show(ex.Message + ": " + ex.stationCode, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string address = addressTextBox.Text;
                int code = int.Parse(codeTextBlock.Text);
                bool disAccess = (disabledAccessCheckBox.IsChecked == true);
                string name = nameTextBox.Text;
                double longitude = double.Parse(longitudeTextBlock.Text);
                double latitude = double.Parse(latitudeTextBlock.Text);
                BO.Station stat = new BO.Station() { Address = address, Code = code, Latitude = latitude, Longitude = longitude, Name = name, DisabledAccess = disAccess };
                bl.UpdateStation(stat);
                MessageBox.Show("The bus was updated successfully", "", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (BO.BadStationCodeException ex)
            {
                MessageBox.Show(ex.Message + ": " + ex.stationCode, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
