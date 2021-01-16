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
    /// Interaction logic for LineDetails.xaml
    /// </summary>
    public partial class LineDetails : Window
    {
        IBL bl;
        BO.Line line;
        public LineDetails(IBL _bl, BO.Line _line)
        {
            InitializeComponent();
            bl = _bl;
            line = _line;
            grid1.DataContext = bl.GetLine(line.LineId);
            areaComboBox.ItemsSource= Enum.GetValues(typeof(BO.Area));
            LBStations.DataContext = line.Stations;
            areaComboBox.Text = line.Area.ToString();
            LBFrequency.DataContext = line.DepTimes;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            System.Windows.Data.CollectionViewSource lineViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("lineViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // lineViewSource.Source = [generic data source]
        }
        public void RefreshAllLine()
        {
            line = bl.GetLine(line.LineId);
            grid1.DataContext = line;
            LBStations.DataContext = line.Stations;
            LBFrequency.DataContext = line.DepTimes;
        }
        private void winUpdate_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            RefreshAllLine();
        }

        private void Button_Click_Add_Station(object sender, RoutedEventArgs e)
        {
            AddStationToLine win=new AddStationToLine(bl,line);
            win.Closing += winUpdate_Closing;
            win.ShowDialog();
        }

        private void Button_Click_Update(object sender, RoutedEventArgs e)
        {
            int lineId = int.Parse(lineIdTextBlock.Text);
            int lineNum = int.Parse(lineNumTextBox.Text);
            BO.Area area= (BO.Area)Enum.Parse(typeof(BO.Area), areaComboBox.SelectedItem.ToString());
            BO.Line lineUpdate = new BO.Line() { LineId = lineId, LineNum = lineNum, Area = area, Stations = line.Stations };
            try
            {
                bl.UpdateLineDetails(lineUpdate);
                Close();
            }
            catch(BO.BadLineIdException ex)
            {
                MessageBox.Show(ex.ToString(), "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void Button_Click_Delete(object sender, RoutedEventArgs e)
        {
            MessageBoxResult res = MessageBox.Show("Are you sure deleting selected line?", "Verification", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (res == MessageBoxResult.No)
                return;
            try
            {
                bl.DeleteLine(line.LineId);
            }
            catch (BO.BadLineIdException ex)
            {
                MessageBox.Show(ex.ToString(), "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            Close();
        }

        private void Delete_Station_Button_Click(object sender, RoutedEventArgs e)
        {
            BO.StationInLine station = (sender as Image).DataContext as BO.StationInLine;
            try
            {
                bl.DeleteLineStation(line.LineId, station.StationCode);
                line = bl.GetLine(line.LineId);
                LBStations.DataContext = line.Stations;//refresh
            }
            catch (BO.BadLineIdException ex)
            {
                MessageBox.Show(ex.Message + ": " + ex.ID, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (BO.BadAdjacentStationsException ex)
            {
                MessageBox.Show(ex.Message + ": " + ex.stationCode1 + " " + ex.stationCode2, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (BO.BadLineStationException ex)
            {
                MessageBox.Show(ex.Message + ": " + ex.lineId + " " + ex.stationCode, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch(BO.BadInputException ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void UpdateTimeDis_Button_Click(object sender, RoutedEventArgs e)
        {
            
            BO.StationInLine st = (sender as Button).DataContext as BO.StationInLine;
            if(st.StationCode==line.Stations[line.Stations.Count-1].StationCode)
            {
                MessageBox.Show("travel distance/time from Last station cant be updated.", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            BO.StationInLine next = line.Stations[st.LineStationIndex];
            UpdateTimeAndDistance win = new UpdateTimeAndDistance(bl, st, next);
            win.Closing += winUpdate_Closing;
            win.ShowDialog();

        }
        private void Delete_Frequency_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                TimeSpan t = (TimeSpan)(sender as Image).DataContext;
                bl.DeleteDepTime(line.LineId, t);
                RefreshAllLine();
            }

            catch (BO.BadLineTripException ex)
            {
                MessageBox.Show(ex.Message+": "+ex.depTime, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Add_Dep_Time_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                TimeSpan t = TimeSpan.Parse(TBNewDepTime.Text);
                bl.AddDepTime(line.LineId, t);
                RefreshAllLine();
            }

            catch (BO.BadLineTripException ex)
            {
                MessageBox.Show(ex.Message + ": " + ex.depTime, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
           

        }
    }
}
