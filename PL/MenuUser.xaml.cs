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
    /// Interaction logic for MenuUser.xaml
    /// </summary>
    public partial class MenuUser : Window
    {
        IBL bl;
        BO.User user;
        public MenuUser(IBL _bl, BO.User _user)
        {
            InitializeComponent();
            bl = _bl;
            user = _user;
        }
    }
}
