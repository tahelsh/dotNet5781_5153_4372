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
    /// Interaction logic for Buses.xaml
    /// </summary>
    public partial class Buses : Window
    {
        IBL bl;
        public Buses(IBL _bl)
        {
            InitializeComponent();
            bl = _bl;
            RefreshAllBusesList();
        }
        public void RefreshAllBusesList()
        {
            List<BO.Bus> buses = bl.GetAllSBuses().ToList();
            LBBuses.DataContext = buses;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AddNewBus win = new AddNewBus(bl);
            win.Closing += winUpdate_Closing;
            win.ShowDialog();
        }
        private void winUpdate_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            RefreshAllBusesList();
        }

        private void Bus_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            BO.Bus b = (sender as ListBox).SelectedItem as BO.Bus;
            BusDetails win = new BusDetails(bl,b);
            win.Closing += winUpdate_Closing;
            win.ShowDialog();
        }
    }
}
