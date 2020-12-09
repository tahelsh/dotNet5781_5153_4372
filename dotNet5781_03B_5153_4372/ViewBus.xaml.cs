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
            BusCurrent.Refuel();
            MessageBox.Show("The bus was refueled successfully.", "Refuel  ", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Treatment_Button(object sender, RoutedEventArgs e)
        {
            BusCurrent.Treatment();
            MessageBox.Show("Treatment was added successfully.", "Treatment  ", MessageBoxButton.OK, MessageBoxImage.Information);
            
        }
    }
}
