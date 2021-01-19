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
    /// Interaction logic for AddStationToLine.xaml
    /// </summary>
    public partial class AddStationToLine : Window
    {
        IBL bl;
        BO.Line line;
        public AddStationToLine(IBL _bl, BO.Line _line)
        {
            InitializeComponent();
            bl = _bl;
            line = _line;
            //show only specific Property of object
            CBPrevStation.DisplayMemberPath = "Name";
            CBNewStation.DisplayMemberPath = "Name";
            CBPrevStation.SelectedIndex = 0; //index of the object to be selected
            CBNewStation.SelectedIndex = 0; //index of the object to be selected
            CBPrevStation.DataContext = line.Stations.ToList();
            CBNewStation.DataContext = bl.GetAllStations();
        }


        private void Button_Click_Submit(object sender, RoutedEventArgs e)
        {
            if (CBIsFirst.IsChecked == true)
            {
                //Station in line
                BO.Station s = (CBNewStation.SelectedItem) as BO.Station;
                BO.LineStation newS = new BO.LineStation() { LineId = line.LineId, LineStationIndex = 1, StationCode = s.Code };
                try
                {
                    bl.AddLineStation(newS);
                    Close();
                }
                catch (BO.BadLineStationException ex)
                {
                    MessageBox.Show(ex.Message+": "+ex.lineId+" "+ex.stationCode, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);//להוסיף הערה
                }
                catch (BO.BadAdjacentStationsException ex)
                {
                    MessageBox.Show(ex.Message + ": " + ex.stationCode1 + " " + ex.stationCode2, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);//להוסיף הערה
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                BO.StationInLine sPrev = (CBPrevStation.SelectedItem) as BO.StationInLine;
                if (sPrev == null)
                {
                    MessageBox.Show("Error", "Error", MessageBoxButton.OK, MessageBoxImage.Error);//להוסיף הערה
                    return;
                }
                BO.Station sNew = (CBNewStation.SelectedItem) as BO.Station;
                BO.LineStation newS = new BO.LineStation() { LineId = line.LineId, LineStationIndex = sPrev.LineStationIndex + 1, StationCode = sNew.Code };
                try
                {
                    bl.AddLineStation(newS);
                    Close();
                }
                catch (BO.BadLineStationException ex)
                {
                    MessageBox.Show(ex.Message + ": " + ex.lineId + " " + ex.stationCode, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);//להוסיף הערה
                }
                catch (BO.BadAdjacentStationsException ex)
                {
                    MessageBox.Show(ex.Message + ": " + ex.stationCode1 + " " + ex.stationCode2, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);//להוסיף הערה
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            
        }
    }
}
