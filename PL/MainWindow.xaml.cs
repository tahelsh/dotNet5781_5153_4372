﻿using System;
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
        IBL bl = BLFactory.GetBL();
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
                if(userName=="")
                {
                    userNameTextBox.BorderBrush = Brushes.Red;
                    MessageBox.Show("Username was not entered", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (Name == "")
                {
                    MessageBox.Show("you need to enter a name", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                //Check if the username doesnt already exist, only if it doesnt, continue input
                int passcode;
                bool success;
                success = int.TryParse(passcodeTextBox.Text, out passcode);
                if (!success || passcode <= 0)
                {
                    passcodeTextBox.BorderBrush = Brushes.Red;
                    MessageBox.Show("The passcode needs to be only with digits", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                bool isAdmin = (adminAccessCheckBox.IsChecked == true);
                BO.User user = new BO.User() { AdminAccess = isAdmin, Name = name, Passcode = passcode, UserName = userName };
                bl.AddUser(user);
                MessageBox.Show("The user was added successfully, good luck:)", "Good Luck!", MessageBoxButton.OK, MessageBoxImage.Information);
                passcodeTextBox.BorderBrush = Brushes.Black;
                userNameTextBox.BorderBrush = Brushes.Black;
            }
            catch (BO.BadUserNameException ex)
            {
                userNameTextBox.BorderBrush = Brushes.Red;
                MessageBox.Show(ex.Message + ": " + ex.userName, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SignInButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string userName = userNameSignTextBox.Text;
                int passcode = int.Parse(PBPasscode.Password);
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
