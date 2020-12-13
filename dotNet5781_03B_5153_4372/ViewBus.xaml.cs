using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for ViewBus.xamlllll
    /// </summary>
    public partial class ViewBus : Window
    {
        public Bus BusCurrent { get; set; }
        public ProgressBar ProgressBar { get; set; }
        public Label Lable { get; set; }
        public ViewBus(Bus b, ProgressBar pd, Label l)
        {
            InitializeComponent();
            BusCurrent = b;
            BusTextBlock.Text = BusCurrent.ToString();
            ProgressBar = pd;
            Lable = l;
        }

        private void Refuel_Button(object sender, RoutedEventArgs e)
        {
            if(BusCurrent.Fuel==1200)
            {
                MessageBox.Show("The fuel tank of the bus is already full", "Worning", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            BusCurrent.Refuel();
            BusTextBlock.Text = BusCurrent.ToString();
            BackgroundWorker workerRefuel = new BackgroundWorker();
            workerRefuel.DoWork += Worker_DoWork;
            workerRefuel.ProgressChanged += Worker_ProgressChanged;
            workerRefuel.RunWorkerCompleted += Worker_RunWorkerCompleted_Treatment;
            workerRefuel.WorkerReportsProgress = true;
            DataTread thread = new DataTread(ProgressBar, Lable, 12, BusCurrent);
            thread.ProgressBar.Visibility = Visibility.Visible;
            thread.Label.Visibility = Visibility.Visible;
            workerRefuel.RunWorkerAsync(thread);
            MessageBox.Show("The bus was refueled successfully.", "Refuel  ", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Treatment_Button(object sender, RoutedEventArgs e)
        {
            if(BusCurrent.BusStatus!=Status.Available)
            {
                MessageBox.Show("The bus can not be treated right now, its not avaliable", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (BusCurrent.LastTreat == DateTime.Now && BusCurrent.KmTreat == BusCurrent.TotalKm)
            {
                MessageBox.Show("The bus was already treatmented", "Worning", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            BackgroundWorker workerRefuel = new BackgroundWorker();
            workerRefuel.DoWork += Worker_DoWork;
            workerRefuel.ProgressChanged += Worker_ProgressChanged;
            workerRefuel.RunWorkerCompleted += Worker_RunWorkerCompleted_Treatment;
            workerRefuel.WorkerReportsProgress = true;
            DataTread thread = new DataTread(ProgressBar, Lable, 144, BusCurrent);
            thread.ProgressBar.Visibility = Visibility.Visible;
            thread.Label.Visibility = Visibility.Visible;
            workerRefuel.RunWorkerAsync(thread);

        }
        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            DataTread data = (DataTread)e.Argument;
            int length = data.Seconds;
            for (int i = 1; i <= length; i++)
            {
                System.Threading.Thread.Sleep(1000);
                (sender as BackgroundWorker).ReportProgress(i, data);
            }
            e.Result = data;
        }
        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            int progress = (int)e.ProgressPercentage;//i
            DataTread data = (DataTread)e.UserState;
            int result = data.Seconds - progress;
            data.Label.Content = result;
            data.ProgressBar.Value = (progress * 100) / data.Seconds;
        }
        private void Worker_RunWorkerCompleted_Treatment(object sender, RunWorkerCompletedEventArgs e)
        {
            BusCurrent.Treatment();
            BusTextBlock.Text = BusCurrent.ToString();
            DataTread data = ((DataTread)(e.Result));
            data.ProgressBar.Visibility = Visibility.Hidden;
            data.Label.Visibility = Visibility.Hidden;
            data.Bus.BusStatus = Status.Available;
            MessageBox.Show("The bus was treated successfully.", "Finished a treatment  ", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
