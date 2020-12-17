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
//using ToolsWPF;
using System.Data;
using System.Drawing;

namespace dotNet5781_03B_5153_4372
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// sdhhdishi
    public partial class MainWindow : Window
    {
        static Random rand = new Random();
        public ObservableCollection<Bus> buses;

        //BackgroundWorker workerRefuel;
        public MainWindow()
        {
            InitializeComponent();
            buses = new ObservableCollection<Bus>();
            RestartBuses.Restart10Buses(buses);
            lbBuses.ItemsSource = buses;
        }

        private void bAddBus_Click(object sender, RoutedEventArgs e)//adding new bus
        {
            AddNewBus win = new AddNewBus();//new window
            win.Buses = buses;//send the list of buses to the new window
            win.ShowDialog();
        }
        private void Refuel(object sender, RoutedEventArgs e)

        {
            Bus b = (sender as Button).DataContext as Bus;//the bus that in the line of the button
            if (b.IsBusy())//if the bus does not avaliable
            {
                MessageBox.Show("The bus can't be refueled right now, it isn't availble", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (b.Fuel == 1200)//if the tank of fuel is already full
            {
                MessageBox.Show("The fuel tank of the bus is already full", "Worning", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            b.BusStatus = Status.Refueling;//change the status of the bus
            BackgroundWorker workerRefuel= new BackgroundWorker(); //new thread
            workerRefuel.DoWork += Worker_DoWork;
            workerRefuel.ProgressChanged += Worker_ProgressChanged;
            workerRefuel.RunWorkerCompleted += Worker_RunWorkerCompleted_Refuel;
            workerRefuel.WorkerReportsProgress = true;
            DataThread thread = new DataThread(Finditem<ProgressBar>((sender as Button).DataContext, "pbTread"), Finditem<Label>((sender as Button).DataContext, "seconds"), Finditem<Button>((sender as Button).DataContext, "StartDrivingButton"), 12, b);//datialsto send to the thread
            thread.UpdateDetails(b.BusStatus);//update the details before the thread
            workerRefuel.RunWorkerAsync(thread);//start the thread

        }
        private void Start_Driving_Button_Click(object sender, RoutedEventArgs e)
        {
            Bus b = (sender as Button).DataContext as Bus;//the bus
            if(b.IsBusy())//if the bus is in thread
            {
                MessageBox.Show("The bus can't start driving right now, it isn't availble", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            StartDrive win = new StartDrive();//new window for enter the distance to travel
            win.Bus = b;
            win.ShowDialog();
            if (win.Bus.BusStatus == Status.Available)//if it cannot drive the ride
                return;
            BackgroundWorker workerRefuel= new BackgroundWorker(); //new thread
            workerRefuel.DoWork += Worker_DoWork;
            workerRefuel.ProgressChanged += Worker_ProgressChanged;
            workerRefuel.RunWorkerCompleted += Worker_RunWorkerCompleted_Driving;
            workerRefuel.WorkerReportsProgress = true;
            int speedTravel = rand.Next(20, 50);//rand speed travel
            int timeTravel = (int)((win.Distance / speedTravel) * 6);//time travel in 
            DataThread thread = new DataThread(Finditem<ProgressBar>((sender as Button).DataContext, "pbTread"), Finditem<Label>((sender as Button).DataContext, "seconds"), sender as Button, timeTravel, b , win.Distance);//details for the thread
            thread.UpdateDetails(win.Bus.BusStatus);//update the details before the thread
            workerRefuel.RunWorkerAsync(thread); //start the thread

        }
        private void ListBoxDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Bus b = (sender as ListBox).SelectedItem as Bus;
            if (b == null)
                return;
            ProgressBar bar = Finditem<ProgressBar>((sender as ListBox).SelectedItem, "pbTread");//the progress bar of the line in the list box that the user did double click
            Label l = Finditem<Label>((sender as ListBox).SelectedItem, "seconds");//the label of time of the line in the list box that the user did double click
            Button button= Finditem<Button>((sender as ListBox).SelectedItem, "StartDrivingButton");//the button of start driving of the line in the list box that the user did double click
            ViewBus win = new ViewBus(b, bar, l, button);//send them to the view bus window
            win.ShowDialog();
        }
        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            DataThread data = (DataThread)e.Argument;
            int length = data.Seconds;
            for (int i = 1; i <= length; i++)
            {
                System.Threading.Thread.Sleep(1000);
                (sender as BackgroundWorker).ReportProgress(i , data);//update the display every a second
            }
            e.Result = data;
        }
        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            int progress = (int)e.ProgressPercentage;//i
            DataThread data = (DataThread)e.UserState;
            int result = data.Seconds -progress;//the time in seconds that stay till the thread will end
            data.Label.Content = result;//update the label
            data.ProgressBar.Value = (progress*100)/data.Seconds;//update the progress bar
        }
        private void Worker_RunWorkerCompleted_Refuel(object sender, RunWorkerCompletedEventArgs e)//complete thread for refuel
        {
            DataThread data = ((DataThread)(e.Result));
            data.Bus.BusStatus = Status.Available;//update the status of the bus
            data.UpdateDetails(data.Bus.BusStatus);//update details after the thread
            data.Bus.Refuel();//update the details of the bus after the refuel
            MessageBox.Show("The bus was refueled successfully.", "Refuel  ", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        private void Worker_RunWorkerCompleted_Driving(object sender, RunWorkerCompletedEventArgs e)
        {
            DataThread data = ((DataThread)(e.Result));
            data.Bus.DoRide(data.DistanceDriving);//update the details of the bus after the driving
            data.Bus.BusStatus = Status.Available;//update the status of the bus
            data.UpdateDetails(data.Bus.BusStatus);//update details after the thread
            MessageBox.Show("The ride went successfully.", "Finished a driving  ", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        //hlep functions for find controls in a specific line of the list box
        //the target is to send something in the line and the name of the item that we want in this line and the fuction will return it
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

    }
}
