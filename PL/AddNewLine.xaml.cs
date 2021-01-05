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
            lastStationComboBox.DisplayMemberPath = "Name";
            firstStationComboBox.SelectedIndex = 0;
            lastStationComboBox.SelectedIndex = 0;
            firstStationComboBox.DataContext = bl.GetAllStations().ToList();
            lastStationComboBox.DataContext = bl.GetAllStations().ToList();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            System.Windows.Data.CollectionViewSource lineViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("lineViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // lineViewSource.Source = [generic data source]
        }

        private void Button_Click_Submit(object sender, RoutedEventArgs e)
        {
            try
            {
                BO.Station firstStation = (firstStationComboBox.SelectedItem) as BO.Station;
                BO.Station lastStation = (lastStationComboBox.SelectedItem) as BO.Station;
                if (bl.IsExistAdjacentStations(firstStation.Code, lastStation.Code))
                {
                    int lineNum = int.Parse(lineNumTextBox.Text);
                    BO.Area area = (BO.Area)Enum.Parse(typeof(BO.Area), areaComboBox.SelectedItem.ToString());
                    BO.Line newline = new BO.Line() { LineId = -1, LineNum = lineNum, Area = area };
                    BO.StationInLine temp1 = new BO.StationInLine() { DisabledAccess = firstStation.DisabledAccess, Name = firstStation.Name, LineStationIndex = 1, StationCode = firstStation.Code };
                    newline.Stations.ToList().Add(temp1);
                    BO.StationInLine temp2 = new BO.StationInLine() { DisabledAccess = lastStation.DisabledAccess, Name = lastStation.Name, LineStationIndex = 2, StationCode = lastStation.Code };
                    bl.AddNewLine(newline);
                    MessageBox.Show("The line was added successfully", "", MessageBoxButton.OK, MessageBoxImage.Information);
                    Close();
                }
                else
                {
                    saveAllbutton.Visibility = Visibility.Visible;
                }
            }
            catch (BO.BadLineIdException ex)
            {
                MessageBox.Show(ex.ToString(), "ERROR ", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR ", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void Button_Click_Finish(object sender, RoutedEventArgs e)
        {
            try
            {
                BO.Station firstStation = (firstStationComboBox.SelectedItem) as BO.Station;
                BO.Station lastStation = (lastStationComboBox.SelectedItem) as BO.Station;
                int lineNum = int.Parse(lineNumTextBox.Text);
                BO.Area area = (BO.Area)Enum.Parse(typeof(BO.Area), areaComboBox.SelectedItem.ToString());
                BO.Line newline = new BO.Line() { LineId = -1, LineNum = lineNum, Area = area };
                BO.StationInLine temp1 = new BO.StationInLine() { DisabledAccess = firstStation.DisabledAccess, Name = firstStation.Name, LineStationIndex = 1, StationCode = firstStation.Code };
                List<BO.StationInLine> list = new List<BO.StationInLine>();
                list.Add(temp1);
                BO.StationInLine temp2 = new BO.StationInLine() { DisabledAccess = lastStation.DisabledAccess, Name = lastStation.Name, LineStationIndex = 2, StationCode = lastStation.Code };
                list.Add(temp2);
                list.Add(temp2);
                newline.Stations = from stat in list
                                   select stat;
                bl.AddNewLine(newline);
                TimeSpan time = TimeSpan.Parse(travelTimeTextBox.Text);
                double distance = double.Parse(travelDistanceTextBox.Text);
                BO.AdjacentStation adj = new BO.AdjacentStation() { StationCode1 = firstStation.Code, StationCode2 = lastStation.Code, Distance = distance, Time = time };
                bl.AddAdjacentStations(adj);
                MessageBox.Show("The line was added successfully", "", MessageBoxButton.OK, MessageBoxImage.Information);
                Close();
            }
            catch (BO.BadLineIdException ex)
            {
                MessageBox.Show(ex.ToString(), "ERROR ", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR ", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
