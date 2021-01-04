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
            //List<BO.Bus> buses = bl.GetAllBuses().ToList();
            //LBBuses.DataContext = buses;
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
            catch(BO.BadLineIdException ex)
            {
                MessageBox.Show(ex.ToString(), "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            Close();
        }
    }
}
