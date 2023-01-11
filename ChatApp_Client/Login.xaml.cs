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

        private void InitData()
        {
            // Clients data

            string json_path = "data/Clients.json";
            if (!File.Exists(json_path))
            {
                return;
            }
            App.clients = JsonSerializer.Deserialize<List<ClientData>>(File.ReadAllText(json_path))!;

            // Groups data

            json_path = "data/Groups.json";
            if (!File.Exists(json_path))
            {
                return;
            }
            App.groups = JsonSerializer.Deserialize<List<GroupData>>(File.ReadAllText(json_path))!;
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
                        App.mainWindow = new(new Client_Process(client.ID));
                        App.mainWindow.Show();
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
            MessageBox.Show("Clicked");
        }
    }
}
