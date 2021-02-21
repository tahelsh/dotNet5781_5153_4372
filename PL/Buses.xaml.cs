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
using System.Windows.Shapes;
using BLAPI;

namespace PL
{
    /// <summary>
    /// Interaction logic for Buses.xaml
    /// </summary>
    public partial class Buses : Window
    {
        IBL bl;
        public Buses(IBL _bl)
        {
            InitializeComponent();
            bl = _bl;
            RefreshAllBusesList();
        }
        public void RefreshAllBusesList()
        {
            List<BO.Bus> buses = bl.GetAllBuses().ToList();
            LBBuses.DataContext = buses;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AddNewBus win = new AddNewBus(bl);
            win.Closing += winUpdate_Closing;
            win.ShowDialog();
        }
        private void winUpdate_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            RefreshAllBusesList();
        }

        private void Bus_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            BO.Bus b = (sender as ListBox).SelectedItem as BO.Bus;
            if (b == null)
                return;
            BusDetails win = new BusDetails(bl,b);
            win.Closing += winUpdate_Closing;
            win.ShowDialog();
        }

        private void Button_Click_DeleteBus(object sender, MouseButtonEventArgs e)
        {
            BO.Bus bus = (sender as Image).DataContext as BO.Bus;
            MessageBoxResult res = MessageBox.Show("Are you sure deleting selected bus?", "Verification", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (res == MessageBoxResult.No)
                return;
            try
            {
                bl.DeleteBus(bus.LicenseNum);
                RefreshAllBusesList();
            }
            catch (BO.BadLicenseNumException ex)
            {
                MessageBox.Show(ex.ToString(), "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        //private void Refuel_Button_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        BO.Bus bus = (sender as Button).DataContext as BO.Bus;
        //        bl.RefuelBus(bus);
        //        RefreshAllBusesList();
        //        MessageBox.Show("The bus was refueled successfully", "ERROR", MessageBoxButton.OK, MessageBoxImage.Information);
        //    }
        //    catch(BO.BadLicenseNumException ex)
        //    {
        //        MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
        //    }
        //    catch (BO.BadInputException ex)
        //    {
        //        MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
        //    }
        //}
        //private void Treatment_Button_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        BO.Bus bus = (sender as Button).DataContext as BO.Bus;
        //        bl.TreatBus(bus);
        //        RefreshAllBusesList();
        //        MessageBox.Show("The bus was treated successfully", "ERROR", MessageBoxButton.OK, MessageBoxImage.Information);
        //    }
        //    catch (BO.BadLicenseNumException ex)
        //    {
        //        MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
        //    }
        //    catch (BO.BadInputException ex)
        //    {
        //        MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
        //    }

        //}
    }
}
