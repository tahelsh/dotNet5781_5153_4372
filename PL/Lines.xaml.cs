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
    /// Interaction logic for Lines.xaml
    /// </summary>
    public partial class Lines : Window
    {
        IBL bl;
        public Lines(IBL _bl)
        {
            InitializeComponent();
            bl = _bl;
            RefreshAllLinesList();
        }

        public void RefreshAllLinesList()
        {
            List<BO.Line> lines = bl.GetAllLines().ToList();
            LBLines.DataContext = lines;
        }

        private void Line_MouseClick(object sender, MouseButtonEventArgs e)
        {
            BO.Line line = (sender as ListBox).SelectedItem as BO.Line;
            if (line == null)
                return;
            LineDetails win = new LineDetails(bl, line);
            win.Closing += winUpdate_Closing;
            win.ShowDialog();
        }
        private void winUpdate_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            RefreshAllLinesList();
        }

        private void Button_Click_AddNewLine(object sender, RoutedEventArgs e)
        {
            AddNewLine win = new AddNewLine(bl);
            win.Closing += winUpdate_Closing;
            win.ShowDialog();

        }
        private void Button_Click_DeleteLine(object sender, RoutedEventArgs e)
        {
            BO.Line line = (sender as Image).DataContext as BO.Line;
            MessageBoxResult res = MessageBox.Show("Are you sure deleting selected line?", "Verification", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (res == MessageBoxResult.No)
                return;
            try
            {
                bl.DeleteLine(line.LineId);
                RefreshAllLinesList();
            }
            catch (BO.BadLineIdException ex)
            {
                MessageBox.Show(ex.ToString(), "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
