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
    /// Interaction logic for AddNewBus.xaml
    /// </summary>
    public partial class AddNewBus : Window
    {
        IBL bl;
        public AddNewBus(IBL _bl)
        {
            InitializeComponent();
            statusComboBox.ItemsSource = Enum.GetValues(typeof(BO.BusStatus));
            statusComboBox.SelectedIndex = 0;
            bl = _bl;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            System.Windows.Data.CollectionViewSource busViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("busViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // busViewSource.Source = [generic data source]
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int licenum = int.Parse(licenseNumTextBox.Text);
                double fuel = double.Parse(fuelRemainTextBox.Text);
                DateTime fromDate = DateTime.Parse(fromDateDatePicker.Text);
                DateTime lastDate = DateTime.Parse(dateLastTreatDatePicker.Text);
                double kmLastTreat = double.Parse(kmLastTreatTextBox.Text);
                BO.BusStatus st = (BO.BusStatus)Enum.Parse(typeof(BO.BusStatus), statusComboBox.SelectedItem.ToString());
                double totalKm = double.Parse(totalTripTextBox.Text);
                BO.Bus b = new BO.Bus() { LicenseNum = licenum, FuelRemain = fuel, FromDate = fromDate, DateLastTreat = lastDate, Status = st, TotalTrip = totalKm, KmLastTreat = kmLastTreat };
                bl.AddBus(b);
                Close();
            }
            catch (BO.BadLicenseNumException ex)
            {
                MessageBox.Show(ex.Message+": "+ex.licenseNum, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (BO.BadInputException ex)
            {
                MessageBox.Show(ex.Message , "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
             
        }
        

        private void keyCheck(object sender, KeyEventArgs e)
        {
            if (((int)e.Key < (int)Key.D0 || (int)e.Key > (int)Key.D9) && ((int)e.Key < (int)Key.NumPad0 || (int)e.Key > (int)Key.NumPad9) && e.Key != Key.OemPeriod && e.Key != Key.Escape && e.Key != Key.Back)
                e.Handled = true;
        }
    }
}
