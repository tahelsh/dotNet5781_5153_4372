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
        //private void TextBox_OnlyNumbers_PreviewKeyDown(object sender, KeyEventArgs e)
        //{
        //    TextBox text = sender as TextBox;
        //    if (text == null) return;
        //    if (e == null) return;

        //    //allow get out of the text box
        //    if (e.Key == Key.Enter || e.Key == Key.Return || e.Key == Key.Tab)
        //        return;

        //    //allow list of system keys (add other key here if you want to allow)
        //    if (e.Key == Key.Escape || e.Key == Key.Back || e.Key == Key.Delete ||
        //        e.Key == Key.CapsLock || e.Key == Key.LeftShift || e.Key == Key.Home
        //     || e.Key == Key.End || e.Key == Key.Insert || e.Key == Key.Down || e.Key == Key.Right || e.Key == Key.OemPeriod)
        //        return;

        //    char c = (char)KeyInterop.VirtualKeyFromKey(e.Key);

        //    //allow control system keys
        //    if (Char.IsControl(c)) return;

        //    //allow digits (without Shift or Alt)
        //    if (Char.IsDigit(c))
        //        if (!(Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightAlt)))
        //            return; //let this key be written inside the textbox

        //    //forbid letters and signs (#,$, %, ...)
        //    e.Handled = true; //ignore this key. mark event as handled, will not be routed to other controls
        //    return;
        //}

        private void keyCheck(object sender, KeyEventArgs e)
        {
            if (((int)e.Key < (int)Key.D0 || (int)e.Key > (int)Key.D9) && ((int)e.Key < (int)Key.NumPad0 || (int)e.Key > (int)Key.NumPad9) && e.Key != Key.OemPeriod && e.Key != Key.Escape && e.Key != Key.Back)
                e.Handled = true;
        }

    }
}
