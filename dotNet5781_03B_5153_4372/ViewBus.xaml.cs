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

namespace dotNet5781_03B_5153_4372
{
    /// <summary>
    /// Interaction logic for ViewBus.xaml
    /// </summary>
    public partial class ViewBus : Window
    {
        public Bus BusCurrent { get; set; }
        public ViewBus(Bus b)
        {
            InitializeComponent();
            BusCurrent = b;
            BusTextBlock.Text = BusCurrent.ToString();
        }

        private void Refuel_Button(object sender, RoutedEventArgs e)
        {
            if(BusCurrent.Fuel==1200)
            {
                MessageBox.Show("The fuel tank of the bus is already full", "Worning", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            BusCurrent.Refuel();
            MessageBox.Show("The bus was refueled successfully.", "Refuel  ", MessageBoxButton.OK, MessageBoxImage.Information);
            BusTextBlock.Text = BusCurrent.ToString();
        }

        private void Treatment_Button(object sender, RoutedEventArgs e)
        {
            if(BusCurrent.BusStatus!=Status.Available)
            {
                MessageBox.Show("The bus can not be treated right now, its not avaliable", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if(BusCurrent.LastTreat == DateTime.Now && BusCurrent.KmTreat== BusCurrent.TotalKm)
            {
                MessageBox.Show("The bus was already treatmented", "Worning", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            BusCurrent.Treatment();
            //MessageBox.Show("Treatment was added successfully.", "Treatment  ", MessageBoxButton.OK, MessageBoxImage.Information);
            BusTextBlock.Text = BusCurrent.ToString();
            ((MainWindow)Application.Current.MainWindow).Treatment(BusCurrent);

        }
    }
}
