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

namespace ChatApp_Client
{
    /// <summary>
    /// Interaction logic for ChooseGroup.xaml
    /// </summary>
    public partial class ChooseGroup : Window
    {
        Client_Process client;
        public ChooseGroup(Client_Process client)
        {
            InitializeComponent();
            this.client = client;

            Greet.Text = "Welcome, " + App.GetClientDisplayName(client.ID).Split(" ")[0];
        }

        private void btnCreateGroup_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
            App.mainWindow = new(client);
            App.mainWindow.Show();
            this.Close();
        }

        private void btnJoinGroup_Click(object sender, RoutedEventArgs e)
        {
            if (btnCreateGroup.Visibility == Visibility.Visible)
            {
                btnCreateGroup.Visibility = Visibility.Collapsed;
                tboxGroupID.Visibility = Visibility.Visible;
                tboxGroupID.Text = "";
                btnJoinGroup.VerticalAlignment = VerticalAlignment.Center;
                btnJoinGroup.HorizontalAlignment = HorizontalAlignment.Center;
            }
            else
            {
                this.Visibility = Visibility.Collapsed;
                App.mainWindow = new(client, int.Parse(tboxGroupID.Text));
                App.mainWindow.Show();
                this.Close();
            }
        }
    }
}
