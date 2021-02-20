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
    /// Interaction logic for UpdateTimeAndDistance.xaml
    /// </summary>
    public partial class UpdateTimeAndDistance : Window
    {
        IBL bl;
        BO.StationInLine first;
        BO.StationInLine second;
        public UpdateTimeAndDistance(IBL _bl, BO.StationInLine _first, BO.StationInLine _second)
        {
            InitializeComponent();
            bl = _bl;
            first = _first;
            second = _second;
            TBTime.Text = first.Time.ToString();
            TBDistance.Text = first.Distance.ToString();
        }

        private void Save_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                TimeSpan time = TimeSpan.Parse(TBTime.Text);
                double distance = double.Parse(TBDistance.Text);
                if (distance < 0)
                {
                    MessageBox.Show("invalid distance", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                //BO.AdjacentStation adj = new BO.AdjacentStation() { StationCode1 = first.StationCode, StationCode2 = second.StationCode, Distance = distance, Time = time }; 
                first.Distance = distance;
                first.Time = time;
                bl.UpdateTimeAndDistance(first,second);
                Close();
            }
            catch (BO.BadAdjacentStationsException ex)
            {
                MessageBox.Show(ex.Message + ": " + ex.stationCode1 + " " + ex.stationCode2, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
        
        private void keyCheck(object sender, KeyEventArgs e)
        {
            if (((int)e.Key < (int)Key.D0 || (int)e.Key > (int)Key.D9) && ((int)e.Key < (int)Key.NumPad0 || (int)e.Key > (int)Key.NumPad9) && e.Key != Key.OemPeriod && e.Key != Key.Escape && e.Key != Key.Back)
                e.Handled = true;
        }

    }
}
