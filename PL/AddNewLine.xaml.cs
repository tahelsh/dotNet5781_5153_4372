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
    /// Interaction logic for AddNewLine.xaml
    /// </summary>
    public partial class AddNewLine : Window
    {
        IBL bl;
        public AddNewLine(IBL _bl)
        {
            InitializeComponent();
            bl = _bl;
            areaComboBox.DataContext = Enum.GetValues(typeof(BO.Area));
            areaComboBox.SelectedIndex = 0;
            firstStationComboBox.DisplayMemberPath = "Name";
            LastStationComboBox.DisplayMemberPath = "Name";
            firstStationComboBox.SelectedIndex = 0;
            LastStationComboBox.SelectedIndex = 0;
            firstStationComboBox.DataContext = bl.GetAllStations().ToList();
            LastStationComboBox.DataContext = bl.GetAllStations().ToList();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            System.Windows.Data.CollectionViewSource lineViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("lineViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // lineViewSource.Source = [generic data source]
        }
    }
}
