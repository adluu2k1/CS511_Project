using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.IO;
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
            InitData();
        }
        public Login(bool isFromAnotherWindow)
        {
            InitializeComponent();
        }

        private void InitData()
        {
            App.UpdateClients();
            App.UpdateGroups();
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
            foreach (var client in App.clients)
            {
                if (tboxUsername.Text == client.Username)
                {
                    if (tboxPassword.Password == client.Pass)
                    {
                        this.Visibility = Visibility.Collapsed;
                        ChooseGroup chooseGroupWindow = new(new Client_Process(client.ID));
                        chooseGroupWindow.Show();
                        this.Close();
                        return;
                    }
                    break;
                }
            }

            MessageBox.Show("Cannot login. Please double check your username and password.", "ChatApp",
                            MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void linkCreateAccount_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
            CreateAccount createAccountWindow = new();
            createAccountWindow.Show();
            this.Close();
            return;
        }
    }
}
