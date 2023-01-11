using Microsoft.Win32;
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
    /// Interaction logic for CreateAccount.xaml
    /// </summary>
    public partial class CreateAccount : Window
    {
        public CreateAccount()
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

        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            // Check

            foreach (var client in App.clients)
            {
                if (!IsEmailValid(tboxEmail.Text))
                {
                    MessageBox.Show("Cannot create account. Invalid email address.", "Create new account",
                                    MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (client.Email == tboxEmail.Text)
                {
                    MessageBox.Show("Cannot create account. This email address has already been taken.", "Create new account",
                                    MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (client.Username == tboxUsername.Text)
                {
                    MessageBox.Show("Cannot create account. This username has already been taken.", "Create new account",
                                    MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }

            // Create

            var new_client = App.CreateClientData(tboxEmail.Text, tboxUsername.Text, tboxPassword.Password, tboxDisplayName.Text, tbAvatarPath.Text);
            App.clients.Add(new_client);
            App.SaveClients();

            // Show MainWindow

            this.Visibility = Visibility.Collapsed;
            App.mainWindow = new(new Client_Process(new_client.ID));
            App.mainWindow.Show();
            this.Close();
        }

        private bool IsEmailValid(string email)
        {
            var trimmedEmail = email.Trim();

            if (trimmedEmail.EndsWith("."))
            {
                return false;
            }
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == trimmedEmail;
            }
            catch
            {
                return false;
            }
        }

        private void linkLogin_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
            Login loginWindow = new(true);
            loginWindow.Show();
            this.Close();
            return;
        }

        private void btnChooseAvatar_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlgOpenFile = new OpenFileDialog();
            dlgOpenFile.RestoreDirectory = true;
            dlgOpenFile.Title = "Choose file";

            if (dlgOpenFile.ShowDialog() == true)
            {
                tbAvatarPath.Text = dlgOpenFile.FileName;
            }
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (tbAvatarPath.Text == "resources/user_male.png")
            {
                tbAvatarPath.Text = "resources/user_female.png";
            }
            else if (tbAvatarPath.Text == "resources/user_female.png")
            {
                tbAvatarPath.Text = "resources/user_male.png";
            }
        }
    }
}
