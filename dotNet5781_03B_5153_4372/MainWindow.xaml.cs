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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using ToolsWPF;

namespace dotNet5781_03B_5153_4372
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<Bus> buses;
        BackgroundWorker workerRefuel;
        public MainWindow()
        {
            InitializeComponent();
            buses = new ObservableCollection<Bus>();
            RestartBuses.Restart10Buses(buses);
            lbBuses.ItemsSource = buses;
            workerRefuel = new BackgroundWorker();
            workerRefuel.DoWork += Worker_DoWork;
            workerRefuel.ProgressChanged += Worker_ProgressChanged;
            workerRefuel.RunWorkerCompleted += Worker_RunWorkerCompleted;
            workerRefuel.WorkerReportsProgress = true;
       
        }

        private void bAddBus_Click(object sender, RoutedEventArgs e)
        {
            AddNewBus win = new AddNewBus();
            win.Buses = buses;
            win.ShowDialog();
        }
        private void Refuel(object sender, RoutedEventArgs e)
        {
            Bus b = (sender as Button).DataContext as Bus;
            b.BusStatus = Status.Refueling;
            DataRefuelTread thread = new DataRefuelTread(lbBuses.GetControl<ProgressBar>(sender as Button, "pbTread"), lbBuses.GetControl<Label>(sender as Button, "seconds"), 12, b);
            thread.ProgressBar.Visibility = Visibility.Visible;
            thread.Label.Visibility = Visibility.Visible;
            workerRefuel.RunWorkerAsync(thread);
        }

        private void Start_Driving_Button_Click(object sender, RoutedEventArgs e)
        {
            Bus b = (sender as Button).DataContext as Bus;
            StartDrive win = new StartDrive();
            win.Bus = b;
            win.ShowDialog();
           // lbBuses.Items.Refresh();
        }

        private void ListBoxDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Bus b = (sender as ListBox).SelectedItem as Bus;
            if (b == null)
                return;
            ViewBus win = new ViewBus(b);
           win.ShowDialog();
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            DataRefuelTread data = (DataRefuelTread)e.Argument;
            int length = data.Seconds;
            for (int i = 1; i <= length; i++)
            {
                System.Threading.Thread.Sleep(1000);
                workerRefuel.ReportProgress(i , data);
            }
            e.Result = data;
        }

        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            int progress = (int)e.ProgressPercentage;//i
            DataRefuelTread data = (DataRefuelTread)e.UserState;
            int result = data.Seconds -progress;
            data.Label.Content = result;
            data.ProgressBar.Value = (progress*100)/data.Seconds;
        }
        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            MessageBox.Show("The bus was refueled successfully.", "Refuel  ", MessageBoxButton.OK, MessageBoxImage.Information);
            DataRefuelTread data = ((DataRefuelTread)(e.Result));
            data.ProgressBar.Visibility = Visibility.Hidden;
            data.Label.Visibility = Visibility.Hidden;
            data.Bus.BusStatus = Status.ReadyToGo;
            data.Bus.Refuel();
        }


    }
}
