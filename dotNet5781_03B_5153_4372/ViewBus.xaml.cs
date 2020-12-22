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
using static System.Windows.Media.Brush;

namespace dotNet5781_03B_5153_4372
{
    /// <summary>
    /// Interaction logic for ViewBus.xaml
    /// </summary>
    public partial class ViewBus : Window
    {
        public Bus BusCurrent { get; set; }//the bus
        public ProgressBar ProgressBar { get; set; }//the progress bar in the line of the bus in the list box in the main window
        public Label Lable { get; set; }//the label in the line of the bus in the list box in the main window
        public Button ButtonDriving { get; set; }//the button of start driving in the line of the bus in the list box in the main window

        public ViewBus(Bus b, ProgressBar pd, Label l, Button button)
        {
            InitializeComponent();
            BusCurrent = b;
            ProgressBar = pd;
            Lable = l;
            ButtonDriving = button;
            BusTextBlock.DataContext = BusCurrent;
        }
        private void Refuel_Button(object sender, RoutedEventArgs e)
        {
            if(BusCurrent.IsBusy())//if the bus does not avaliable
            {
                MessageBox.Show("The bus can't be refueled right now, it isn't availble", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if(BusCurrent.Fuel==1200)//if the tank of the fuel is already full
            {
                MessageBox.Show("The fuel tank of the bus is already full", "Worning", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            BusCurrent.BusStatus = Status.Refueling;//change the status
            BackgroundWorker workerRefuel = new BackgroundWorker();//new thread
            workerRefuel.DoWork += Worker_DoWork;
            workerRefuel.ProgressChanged += Worker_ProgressChanged;
            workerRefuel.RunWorkerCompleted += Worker_RunWorkerCompleted_Fuel;
            workerRefuel.WorkerReportsProgress = true;
            DataThread thread = new DataThread(ProgressBar, Lable, ButtonDriving, 12, BusCurrent);//details to the thread
            thread.UpdateDetails(BusCurrent.BusStatus);//update details by the status
            ProgressBarView.Visibility= Visibility.Visible;//update the progress bar in this window
            ProgressBarView.Foreground = Brushes.Yellow;//update the color of the progress bar in this window
            LabelView.Visibility = Visibility.Visible;//update the label of time in this window
            BusTextBlock.Text = BusCurrent.ToString();//update the display
            workerRefuel.RunWorkerAsync(thread);//start the thread
        }
        private void Treatment_Button(object sender, RoutedEventArgs e)
        {
            if(BusCurrent.IsBusy())//if the bus does not avaliable
            {
                MessageBox.Show("The bus can not be treated right now, its not avaliable", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (BusCurrent.LastTreat.ToShortDateString() == DateTime.Now.ToShortDateString() && BusCurrent.KmTreat == BusCurrent.TotalKm)//if the treatment already done
            {
                MessageBox.Show("The bus was already treatmented", "Worning", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            BusCurrent.BusStatus = Status.Treatment;//change status of the bus
            BackgroundWorker workerTreatment = new BackgroundWorker();//new thread
            workerTreatment.DoWork += Worker_DoWork;
            workerTreatment.ProgressChanged += Worker_ProgressChanged;
            workerTreatment.RunWorkerCompleted += Worker_RunWorkerCompleted_Treatment;
            workerTreatment.WorkerReportsProgress = true;
            DataThread thread = new DataThread(ProgressBar, Lable, ButtonDriving, 144, BusCurrent);//details to the thread
            thread.UpdateDetails(BusCurrent.BusStatus);
            ProgressBarView.Visibility = Visibility.Visible;//update the progress bar in this window
            ProgressBarView.Foreground = Brushes.DeepPink;//update the diaplay
            LabelView.Visibility = Visibility.Visible;//update the label of time in this window
            BusTextBlock.Text = BusCurrent.ToString();//update the display
            workerTreatment.RunWorkerAsync(thread);//start the thread

        }
        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            DataThread data = (DataThread)e.Argument;
            int length = data.Seconds;
            for (int i = 1; i <= length; i++)
            {
                System.Threading.Thread.Sleep(1000);
                (sender as BackgroundWorker).ReportProgress(i, data);//update the display every a second
            }
            e.Result = data;
           
        }
        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            int progress = (int)e.ProgressPercentage;//i
            DataThread data = (DataThread)e.UserState;
            int result = data.Seconds - progress;//the seconds that stay till the thread will end
            data.Label.Content = result;//content of label of the main window
            LabelView.Content = data.Label.Content;//content of label of the this window
            data.ProgressBar.Value = (progress * 100) / data.Seconds;//content of ProgressBar of the main window
            ProgressBarView.Value = data.ProgressBar.Value;//content of ProgressBar of the this window
        }
        private void Worker_RunWorkerCompleted_Treatment(object sender, RunWorkerCompletedEventArgs e)//complete the treatment
        {
            DataThread data = ((DataThread)(e.Result));
            BusCurrent.BusStatus = Status.Available;//change the status
            data.UpdateDetails(BusCurrent.BusStatus);//update the details of the thread in the main window after the thread
            ProgressBarView.Visibility = Visibility.Hidden;//update the ProgressBar of this window
            ProgressBarView.Value = 0;
            LabelView.Visibility = Visibility.Hidden;//update the label of this window
            LabelView.Content = "time";
            BusCurrent.Treatment();//update details after treatment
            BusCurrent.Refuel();//update details after refuel
            BusTextBlock.Text = BusCurrent.ToString();//update the display
            MessageBox.Show("The bus was treated successfully.", "Finished a treatment  ", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        private void Worker_RunWorkerCompleted_Fuel(object sender, RunWorkerCompletedEventArgs e)
        {
            
            DataThread data = ((DataThread)(e.Result));
            ProgressBarView.Visibility = Visibility.Hidden;//update the ProgressBar of this window
            ProgressBarView.Value = 0;
            LabelView.Visibility = Visibility.Hidden;//update the label of this window
            LabelView.Content = "time";
            data.Bus.BusStatus = Status.Available;//change the status
            data.UpdateDetails(BusCurrent.BusStatus);//update the details of the thread in the main window after the thread
            BusCurrent.Refuel();//update details after refuel
            BusTextBlock.Text = BusCurrent.ToString();//update the display
            MessageBox.Show("The bus was refueled successfully.", "Refuel  ", MessageBoxButton.OK, MessageBoxImage.Information);
            
        }
    }
    
}
