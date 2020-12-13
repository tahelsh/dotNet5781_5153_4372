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
using System.Data;
using System.Drawing;

namespace dotNet5781_03B_5153_4372
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// cikdhnkncd
    public partial class MainWindow : Window
    {
        static Random rand = new Random();
        public ObservableCollection<Bus> buses;

        public static object Application { get; internal set; }

        //BackgroundWorker workerRefuel;
        public MainWindow()
        {
            InitializeComponent();
            buses = new ObservableCollection<Bus>();
            RestartBuses.Restart10Buses(buses);
            lbBuses.ItemsSource = buses;
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
            if (b.BusStatus!=Status.Available)
            {
                MessageBox.Show("The bus can't be refueled right now, it isn't availble", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            b.BusStatus = Status.Refueling;
            BackgroundWorker workerRefuel= new BackgroundWorker(); 
            workerRefuel.DoWork += Worker_DoWork;
            workerRefuel.ProgressChanged += Worker_ProgressChanged;
            workerRefuel.RunWorkerCompleted += Worker_RunWorkerCompleted_Refuel;
            workerRefuel.WorkerReportsProgress = true;
            DataTread thread = new DataTread(lbBuses.GetControl<ProgressBar>(sender as Button, "pbTread"), lbBuses.GetControl<Label>(sender as Button, "seconds"), 12, b);
            thread.ProgressBar.Visibility = Visibility.Visible;
            thread.Label.Visibility = Visibility.Visible;
            workerRefuel.RunWorkerAsync(thread);
        }

        private void Start_Driving_Button_Click(object sender, RoutedEventArgs e)
        {
            Bus b = (sender as Button).DataContext as Bus;//the bus
            if(b.BusStatus != Status.Available)//if the bus is in thread
            {
                MessageBox.Show("The bus can't start driving right now, it isn't availble", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            StartDrive win = new StartDrive();
            win.Bus = b;
            win.ShowDialog();
            if (win.Bus.BusStatus == Status.Available)//if it cannot drive the ride
                return;
            BackgroundWorker workerRefuel= new BackgroundWorker(); 
            workerRefuel.DoWork += Worker_DoWork;
            workerRefuel.ProgressChanged += Worker_ProgressChanged;
            workerRefuel.RunWorkerCompleted += Worker_RunWorkerCompleted_Driving;
            workerRefuel.WorkerReportsProgress = true;
            int speedTravel = rand.Next(20, 50);//rand speed travel
            int timeTravel = (int)((win.Distance / speedTravel) * 6);//time travel in seconds
            DataTread thread = new DataTread(lbBuses.GetControl<ProgressBar>(sender as Button, "pbTread"), lbBuses.GetControl<Label>(sender as Button, "seconds"), timeTravel, b);//thread of driving
            thread.ProgressBar.Visibility = Visibility.Visible;
            thread.Label.Visibility = Visibility.Visible;
            workerRefuel.RunWorkerAsync(thread); 
            //lbBuses.Items.Refresh();
        }

        private void ListBoxDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //MainWindow.add progressbar
            Bus b = (sender as ListBox).SelectedItem as Bus;
            if (b == null)
                return;
            ProgressBar bar = Finditem<ProgressBar>((sender as ListBox).SelectedItem, "pbTread");
            Label l = Finditem<Label>((sender as ListBox).SelectedItem, "seconds");
            ViewBus win = new ViewBus(b, bar,l);
            win.ShowDialog();
        }

       //public void Treatment(Bus b)
       //{
       //     BackgroundWorker workerRefuel = new BackgroundWorker();
       //     workerRefuel.DoWork += Worker_DoWork;
       //     workerRefuel.ProgressChanged += Worker_ProgressChanged_Treatment;
       //     workerRefuel.RunWorkerCompleted += Worker_RunWorkerCompleted_Treatment;
       //     workerRefuel.WorkerReportsProgress = true;
       //     b.BusStatus = Status.Treatment;
       //     DataTread thread = new DataTread(144,b);
       //     workerRefuel.RunWorkerAsync(thread);
       //     lbBuses.Items.Refresh();
       // }


        

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            DataTread data = (DataTread)e.Argument;
            int length = data.Seconds;
            for (int i = 1; i <= length; i++)
            {
                System.Threading.Thread.Sleep(1000);
                (sender as BackgroundWorker).ReportProgress(i , data);
            }
            e.Result = data;
        }
        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            int progress = (int)e.ProgressPercentage;//i
            DataTread data = (DataTread)e.UserState;
            int result = data.Seconds -progress;
            data.Label.Content = result;
            data.ProgressBar.Value = (progress*100)/data.Seconds;
        }
        private void Worker_RunWorkerCompleted_Refuel(object sender, RunWorkerCompletedEventArgs e)
        {
            MessageBox.Show("The bus was refueled successfully.", "Refuel  ", MessageBoxButton.OK, MessageBoxImage.Information);
            DataTread data = ((DataTread)(e.Result));
            data.ProgressBar.Visibility = Visibility.Hidden;
            data.Label.Visibility = Visibility.Hidden;
            data.Bus.BusStatus = Status.Available;
            data.Bus.Refuel();
        }
        private void Worker_RunWorkerCompleted_Driving(object sender, RunWorkerCompletedEventArgs e)
        {
            MessageBox.Show("The ride went successfully.", "Finished a driving  ", MessageBoxButton.OK, MessageBoxImage.Information);
            DataTread data = ((DataTread)(e.Result));
            data.ProgressBar.Visibility = Visibility.Hidden;
            data.Label.Visibility = Visibility.Hidden;
            data.Bus.BusStatus = Status.Available;
        }
        private childItem FindVisualChild<childItem>(DependencyObject obj) where childItem : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (child != null && child is childItem)
                {
                    return (childItem)child;
                }
                else
                {
                    childItem childOfChild = FindVisualChild<childItem>(child);
                    if (childOfChild != null)
                        return childOfChild;
                }
            }
            return null;
        }
        public A Finditem<A>(object item, string str)
        {

            ListBoxItem myListBoxItem = (ListBoxItem)(lbBuses.ItemContainerGenerator.ContainerFromItem(item));

            // Getting the ContentPresenter of myListBoxItem
            ContentPresenter myContentPresenter = FindVisualChild<ContentPresenter>(myListBoxItem);

            // Finding textBlock from the DataTemplate that is set on that ContentPresenter
            DataTemplate myDataTemplate = myContentPresenter.ContentTemplate;
            A myLabel = (A)myDataTemplate.FindName(str, myContentPresenter);
            return myLabel;
        }












        //את כל זה סתם עשיתי בשביל הטיפול

        //private void Worker_ProgressChanged_Treatment(object sender, ProgressChangedEventArgs e)
        //{
        //    int progress = (int)e.ProgressPercentage;//i
        //    DataTread data = (DataTread)e.UserState;
        //    data.Bus.ProgressBarTime = (progress * 100) / data.Seconds;
        //    lbBuses.Items.Refresh();
        //}

        //private void Worker_RunWorkerCompleted_Treatment(object sender, RunWorkerCompletedEventArgs e)
        //{
        //    MessageBox.Show("The bus was treated successfully.", "Finished a treatment  ", MessageBoxButton.OK, MessageBoxImage.Information);
        //    DataTread data = ((DataTread)(e.Result));
        //    //data.ProgressBar.Visibility = Visibility.Hidden;
        //    //data.Label.Visibility = Visibility.Hidden;
        //    data.Bus.BusStatus = Status.Available;
        //    data.Bus.ProgressBarTime = 0;
        //}

        //public void func()
        //{
        //    foreach (Bus bus in lbBuses.Items)
        //    {
        //        bus.Attributes.CssStyle.Add("color", "red");


        //    }

        //    //lbBuses.BackColor = Color.Beige;
        //    //lbBuses.DrawMode = DrawMode.OwnerDrawFixed;
        //    //lbBuses.DrawItem += new DrawItemEventHandler(listBox1_SetColor);
        //}

        // public void Treatment()
        // {

        // }
    }
}
