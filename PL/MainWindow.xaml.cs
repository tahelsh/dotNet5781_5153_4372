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
using System.Windows.Navigation;
using System.Windows.Shapes;
using BLAPI;

namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        IBL bl = BLFactory.GetBL("1");
        public MainWindow()
        {
            InitializeComponent();
        }

 

     
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // userViewSource.Source = [generic data source]
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string name = nameTextBox.Text;
                string userName = userNameTextBox.Text;
                //Check if the username doesnt already exist, only if it doesnt, continue input
                int passcode = int.Parse(passcodeTextBox.Text);
                bool isAdmin = (adminAccessCheckBox.IsChecked == true);
                BO.User user = new BO.User() { AdminAccess = isAdmin, Name = name, Passcode = passcode, UserName = userName };
                bl.AddUser(user);
            }
            catch(BO.BadUserNameException ex)
            {
                MessageBox.Show(ex.Message + ": " + ex.userName, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void SignInButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string userName = userNameSignTextBox.Text;
                int passcode = int.Parse(PasscodeSignTextBox.Text);
                BO.User user = bl.SignIn(userName, passcode);
                if (user.AdminAccess)
                {
                    MenuAdmin winAdmin = new MenuAdmin(bl, user);
                    winAdmin.ShowDialog();
                }
                else
                {
                    MenuUser winUser = new MenuUser(bl, user);
                    winUser.ShowDialog();
                }
                
            }
            catch(BO.BadUserNameException )
            {
                MessageBox.Show("The user name or the passcode is wrong", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
