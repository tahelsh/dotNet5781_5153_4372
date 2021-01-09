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
    /// Interaction logic for AddNewStation.xaml
    /// </summary>
    public partial class AddNewStation : Window
    {
        IBL bl;
        public AddNewStation(IBL _bl)
        {
            InitializeComponent();
            bl = _bl;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            System.Windows.Data.CollectionViewSource stationViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("stationViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // stationViewSource.Source = [generic data source]
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string address = addressTextBox.Text;
                int code = int.Parse(codeTextBox.Text);
                bool disAccess = (disabledAccessCheckBox.IsChecked==true);
                string name = nameTextBox.Text;
                double longitude = double.Parse(longitudeTextBox.Text);
                double latitude = double.Parse(latitudeTextBox.Text);
                BO.Station stat = new BO.Station() { Address = address, Code = code, DisabledAccess = disAccess, Name = name, Latitude=latitude, Longitude=longitude };
                bl.AddStation(stat);
                Close();
            }
            catch(BO.BadStationCodeException ex)
            {
                MessageBox.Show(ex.Message+": "+ex.stationCode, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
    }
}
