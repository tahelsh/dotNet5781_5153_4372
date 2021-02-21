using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
        BackgroundWorker worker;
        public BusDetails(IBL _bl, BO.Bus _bus)
        {
            InitializeComponent();
            bl = _bl;
            bus = _bus;
            statusComboBox.ItemsSource = Enum.GetValues(typeof(BO.BusStatus));
            busDetailsGrid.DataContext = bl.GetBus(bus.LicenseNum);
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
            catch (BO.BadLicenseNumException ex)
            {
                MessageBox.Show(ex.Message+": "+ex.licenseNum, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (BO.BadInputException ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error); }
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
            catch (BO.BadLicenseNumException ex)
            {
                MessageBox.Show(ex.Message + ": " + ex.licenseNum, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Operation Failure", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void keyCheck(object sender, KeyEventArgs e)
        {
            if (((int)e.Key < (int)Key.D0 || (int)e.Key > (int)Key.D9) && ((int)e.Key < (int)Key.NumPad0 || (int)e.Key > (int)Key.NumPad9) && e.Key != Key.OemPeriod && e.Key != Key.Escape && e.Key != Key.Back)
                e.Handled = true;
        }

        private void BRefuel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (bus.FuelRemain == 1200 )
                {
                    MessageBox.Show("The fuel remain of the bus ia already full", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if(bus.Status!=BO.BusStatus.Available)
                {
                    MessageBox.Show("The bus does not avaliable", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                bus.Status = BO.BusStatus.Refueling;//change status
                bl.UpdateBusDetails(bus);//update
                worker = new BackgroundWorker();
                worker.DoWork += Worker_DoWork;
                worker.ProgressChanged += Worker_ProgressChanged_fuel;
                worker.RunWorkerCompleted += Worker_RunWorkerCompleted_Refuel;
                worker.WorkerReportsProgress = true;//יכול לדווח על שינויים למסך
                pbRefuel.Visibility = Visibility.Visible;
                worker.RunWorkerAsync(12);// Start the thread.
            }
            catch (BO.BadLicenseNumException ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (BO.BadInputException ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BTreat_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (bus.DateLastTreat.ToShortDateString() == DateTime.Now.ToShortDateString())//if the bus is already treated
                {
                    MessageBox.Show("The bus is already treated today", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (bus.Status != BO.BusStatus.Available)
                {
                    MessageBox.Show("The bus does not avaliable", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                bus.Status = BO.BusStatus.Treatment;//change status
                bl.UpdateBusDetails(bus);//update with the new status
                worker = new BackgroundWorker();
                worker.DoWork += Worker_DoWork;
                worker.ProgressChanged += Worker_ProgressChanged_treat;
                worker.RunWorkerCompleted += Worker_RunWorkerCompleted_Treat;
                worker.WorkerReportsProgress = true;
                pbTreat.Visibility = Visibility.Visible;
                worker.RunWorkerAsync(12); // Start the thread.
            }
            catch (BO.BadLicenseNumException ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (BO.BadInputException ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }





        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            int length = (int)e.Argument;//how many seconds the thread is active
            for (int i = 1; i <= length; i++)
            {
                if (worker.CancellationPending == true)//לא כל כך צריך שעון לפרוייקט שלנו
                {
                    e.Cancel = true;
                    e.Result = stopwatch.ElapsedMilliseconds; // Unnecessary
                    break;
                }
                else
                {
                    // Perform a time consuming operation and report progress.
                    System.Threading.Thread.Sleep(500);
                    worker.ReportProgress(i * 100 / length);
                }
            }

            e.Result = stopwatch.ElapsedMilliseconds;
        }

        private void Worker_ProgressChanged_fuel(object sender, ProgressChangedEventArgs e)
        {
            int progress = e.ProgressPercentage;
            pbRefuel.Value = progress;
        }
        private void Worker_ProgressChanged_treat(object sender, ProgressChangedEventArgs e)
        {
            int progress = e.ProgressPercentage;
            pbTreat.Value = progress;
        }

        private void Worker_RunWorkerCompleted_Refuel(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                pbRefuel.Visibility = Visibility.Hidden;
                bl.RefuelBus(bus);//refuel
                bus = bl.GetBus(bus.LicenseNum);
                busDetailsGrid.DataContext = bus;
                statusComboBox.Text = bus.Status.ToString();//to show the current bus status
                MessageBox.Show("The bus was refuled successfuly", "success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (BO.BadLicenseNumException ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (BO.BadInputException ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void Worker_RunWorkerCompleted_Treat(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                pbTreat.Visibility = Visibility.Hidden;
                bl.TreatBus(bus);//treat the bus
                bus = bl.GetBus(bus.LicenseNum);
                busDetailsGrid.DataContext = bus;//update the grid with the details of the bus after the treatment
                statusComboBox.Text = bus.Status.ToString();//to show the current bus status
                MessageBox.Show("The bus was treated successfuly", "success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (BO.BadLicenseNumException ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (BO.BadInputException ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
