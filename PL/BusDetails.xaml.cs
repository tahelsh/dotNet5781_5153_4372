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
    /// Interaction logic for BusDetails.xaml
    /// </summary>
    public partial class BusDetails : Window
    {
        IBL bl;
        BO.Bus bus;
        public BusDetails(IBL _bl, BO.Bus _bus)
        {
            InitializeComponent();
            bl = _bl;
            bus = _bus;
            statusComboBox.ItemsSource = Enum.GetValues(typeof(BO.BusStatus));
            //statusComboBox.SelectedIndex = 0;
            grid2.DataContext = bl.GetBus(bus.LicenseNum);
            statusComboBox.Text = bus.Status.ToString();//to show the current bus status
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            System.Windows.Data.CollectionViewSource busViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("busViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // busViewSource.Source = [generic data source]
        }

        private void ButtonUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int licenum = int.Parse(licenseNumTextBlock.Text);
                double fuel = double.Parse(fuelRemainTextBox.Text);
                DateTime fromDate = DateTime.Parse(fromDateTextBlock.Text);
                DateTime lastDate = DateTime.Parse(dateLastTreatDatePicker.Text);
                double kmLastTreat = double.Parse(kmLastTreatTextBox.Text);
                BO.BusStatus st = (BO.BusStatus)Enum.Parse(typeof(BO.BusStatus), statusComboBox.SelectedItem.ToString());
                double totalKm = double.Parse(totalTripTextBox.Text);
                BO.Bus b = new BO.Bus() { LicenseNum = licenum, FuelRemain = fuel, FromDate = fromDate, DateLastTreat = lastDate, Status = st, TotalTrip = totalKm, KmLastTreat = kmLastTreat };
                bl.UpdateBusDetails(b);
            }
            catch(Exception ex) { MessageBox.Show("errorrrrrrr", "", MessageBoxButton.OK, MessageBoxImage.Error); }
            Close();
            
        }

        private void ButtonRemove_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult res = MessageBox.Show("Are you sure deleting selected bus?", "Verification", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (res == MessageBoxResult.No)
                return;
            try
            {
                if (bus != null)
                {
                    bl.DeleteBus(bus.LicenseNum);
                    Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Operation Failure", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
