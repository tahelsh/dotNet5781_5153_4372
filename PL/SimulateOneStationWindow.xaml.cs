using BLAPI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PL
{
    /// <summary>
    /// Interaction logic for SimulateOneStationWindow.xaml
    /// </summary>
    public partial class SimulateOneStationWindow : Window
    {
        IBL bl;
        BO.Station currentStation;
        ObservableCollection<BO.LineTiming> lineTimingList;
        Stopwatch stopwatch;
        BackgroundWorker timerworker;
        TimeSpan tsStartTime;
        bool isTimerRun;
        public SimulateOneStationWindow(IBL _bl, BO.Station station)
        {
            InitializeComponent();
            bl = _bl;
            currentStation = station;
            gridOneStation.DataContext = currentStation;
            stopwatch = new Stopwatch();
            timerworker = new BackgroundWorker();
            timerworker.DoWork += Worker_DoWork;
            timerworker.ProgressChanged += Worker_ProgressChanged;
            timerworker.WorkerReportsProgress = true;
            tsStartTime = DateTime.Now.TimeOfDay;
            stopwatch.Restart();
            isTimerRun = true;

            //הוספנו מעצמינו
            LBLineTiming.DataContext = lineTimingList;
            this.Closing += Window_Closing;

            timerworker.RunWorkerAsync();
            
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            stopwatch.Stop();
            isTimerRun = false;
        }

        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            TimeSpan tsCurrentTime = tsStartTime + stopwatch.Elapsed;
            string timmerText = tsCurrentTime.ToString().Substring(0, 8);
            this.timerTextBlock.Text = timmerText;
            //לממש את הפונקציה!
            LBLineTiming.DataContext = bl.GetLineTimingPerStation(currentStation, tsCurrentTime).ToList();
            lineTimingList = new ObservableCollection<BO.LineTiming>( bl.GetLineTimingPerStation(currentStation, tsCurrentTime)); //התצוגה תתעדכן כי זה אובזרוובל קוללקשיין
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            while(isTimerRun)
            {
                timerworker.ReportProgress(231);
                Thread.Sleep(1000);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            System.Windows.Data.CollectionViewSource stationViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("stationViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // stationViewSource.Source = [generic data source]
        }
    }
}
