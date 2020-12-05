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

namespace dotNet5781_03B_5153_4372
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<Bus> buses;
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
            FrameworkElement fxElt = sender as FrameworkElement;
            Bus b = fxElt.DataContext as Bus;
            b.Refuel();
            //MessageBox.Show("The bus was refueled successfully.", "Refuel  ", MessageBoxImage.OK, MessageBoxImage.Information);
        }

        private void Start_Driving_Button_Click(object sender, RoutedEventArgs e)
        {
            StartDrive win = new StartDrive();
            win.ShowDialog();
        }
    }
}
