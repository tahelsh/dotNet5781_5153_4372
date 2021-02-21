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
                if(firstStation.Code==lastStation.Code)
                {
                    MessageBox.Show("The first station and the last station are the same", "ERROR ", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (bl.IsExistAdjacentStations(firstStation.Code, lastStation.Code))
                {
                    int lineNum = int.Parse(lineNumTextBox.Text);
                    BO.Area area = (BO.Area)Enum.Parse(typeof(BO.Area), areaComboBox.SelectedItem.ToString());
                    BO.Line newline = new BO.Line() { LineId = -1, LineNum = lineNum, Area = area, Stations = new List<BO.StationInLine>() };
                    //the first station
                    BO.StationInLine temp1 = new BO.StationInLine() { DisabledAccess = firstStation.DisabledAccess, Name = firstStation.Name, LineStationIndex = 1, StationCode = firstStation.Code};
                    newline.Stations.Add(temp1);
                    //the second station
                    BO.StationInLine temp2 = new BO.StationInLine() { DisabledAccess = lastStation.DisabledAccess, Name = lastStation.Name, LineStationIndex = 2, StationCode = lastStation.Code };
                    newline.Stations.Add(temp2);
                    bl.AddNewLine(newline);//add the new line
                    Close();
                    MessageBox.Show("The line was added successfully", "", MessageBoxButton.OK, MessageBoxImage.Information);
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
            catch(BO.BadInputException ex)
            {
                MessageBox.Show(ex.Message, "ERROR ", MessageBoxButton.OK, MessageBoxImage.Error);
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
                TimeSpan time = TimeSpan.Parse(travelTimeTextBox.Text);
                double distance = double.Parse(travelDistanceTextBox.Text);
                //the first station
                BO.StationInLine temp1 = new BO.StationInLine() { Distance = distance, Time = time, DisabledAccess = firstStation.DisabledAccess, Name = firstStation.Name, LineStationIndex = 1, StationCode = firstStation.Code };
                newline.Stations = new List<BO.StationInLine>();
                newline.Stations.Add(temp1);
                //the second station
                BO.StationInLine temp2 = new BO.StationInLine() { DisabledAccess = lastStation.DisabledAccess, Name = lastStation.Name, LineStationIndex = 2, StationCode = lastStation.Code };
                newline.Stations.Add(temp2);
                bl.AddNewLine(newline);//add the new line
                MessageBox.Show("The line was added successfully", "", MessageBoxButton.OK, MessageBoxImage.Information);
                Close();
            }
            catch (BO.BadLineIdException ex)
            {
                MessageBox.Show(ex.ToString(), "ERROR ", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (BO.BadInputException ex)
            {
                MessageBox.Show(ex.Message, "ERROR ", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR ", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
        private void keyCheck(object sender, KeyEventArgs e)
        {
            if (((int)e.Key < (int)Key.D0 || (int)e.Key > (int)Key.D9) && ((int)e.Key < (int)Key.NumPad0 || (int)e.Key > (int)Key.NumPad9) && e.Key != Key.OemPeriod && e.Key != Key.Escape && e.Key != Key.Back)
                e.Handled = true;
        }
    }
}
