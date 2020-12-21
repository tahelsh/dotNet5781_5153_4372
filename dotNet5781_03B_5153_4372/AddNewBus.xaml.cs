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

namespace dotNet5781_03B_5153_4372
{
    /// <summary>
    /// Interaction logic for AddNewBus.xaml
    /// Tje class builds a window which enables to add a new bus to the existing buses list.
    /// </summary>
    public partial class AddNewBus : Window
    {
        public ObservableCollection<Bus> Buses { get; set; }//a collection of the buses.
        public AddNewBus()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            System.Windows.Data.CollectionViewSource busViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("busViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // busViewSource.Source = [generic data source]
        }

        private void bSubmit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Bus b = new Bus(licNumTextBox.Text, dateStartDatePicker.DisplayDate, double.Parse(totalKmTextBox.Text), double.Parse(fuelTextBox.Text), lastTreatDatePicker.DisplayDate, double.Parse(kmTreatTextBox.Text));
                if (IsExist(b.LicNum))
                {
                    MessageBox.Show("ERROR, there is already bus with this License Number ", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    Buses.Add(b);
                    Close();
                }
            }
            catch (BusException ex)
            {
                MessageBox.Show("ERROR, " + ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }   
        }

        /// <summary>
        /// checks if the bus that the user want to add is already exists
        /// </summary>
        /// <param name="licNum">the license Number of the bus</param>
        /// <returns></returns>
        private bool IsExist(string licNum)
        {
            foreach(Bus b in Buses)
            {
                if (b.LicNum == licNum)
                    return true;
            }
            return false;
        }
    }
}
