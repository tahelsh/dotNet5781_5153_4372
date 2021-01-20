using BLAPI;
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

namespace PL
{
    /// <summary>
    /// Interaction logic for MenuAdmin.xaml
    /// </summary>
    public partial class MenuAdmin : Window
    {
        IBL bl;
        BO.User user;
        public MenuAdmin(IBL _bl, BO.User _user)
        {
            InitializeComponent();
            bl = _bl;
            user = _user;
            LabelUserName.Content = "Hi "+user.Name+", you can choose from the folowing options";
        }
        private void Button_Click_Buses(object sender, RoutedEventArgs e)
        {
            Buses win = new Buses(bl);
            win.Show();
        }

        private void Button_Click_Lines(object sender, RoutedEventArgs e)
        {
            Lines win = new Lines(bl);
            win.Show();
        }

        private void Button_Click_Stations(object sender, RoutedEventArgs e)
        {
            Stations win = new Stations(bl);
            win.Show();
        }
    }
}
