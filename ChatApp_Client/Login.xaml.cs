using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
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

namespace ChatApp_Client
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }

        private void tboxPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (tboxPassword.Password == "")
            {
                placeholderPassword.Visibility = Visibility.Visible;
            }
            else
            {
                placeholderPassword.Visibility = Visibility.Collapsed;
            }
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {

        }

        private void linkCreateAccount_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("Clicked");
        }
    }
}
