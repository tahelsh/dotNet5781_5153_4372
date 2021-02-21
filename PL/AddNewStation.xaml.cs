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
                string address = addressTextBox.Text;//address
                int code = int.Parse(codeTextBox.Text);//code 
                bool disAccess = (disabledAccessCheckBox.IsChecked==true);//disable access
                string name = nameTextBox.Text;//name of station
                double longitude = double.Parse(longitudeTextBox.Text);//longitude
                double latitude = double.Parse(latitudeTextBox.Text);//latitude
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
                MessageBox.Show(ex.Message, "ERROR Input", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
        private void keyCheck(object sender, KeyEventArgs e)//the function enable to emter only digits and point
        {
            if (((int)e.Key < (int)Key.D0 || (int)e.Key > (int)Key.D9) && ((int)e.Key < (int)Key.NumPad0 || (int)e.Key > (int)Key.NumPad9) && e.Key != Key.OemPeriod && e.Key != Key.Escape && e.Key != Key.Back)
                e.Handled = true;
        }
    }
}
