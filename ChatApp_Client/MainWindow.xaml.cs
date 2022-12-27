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

namespace ChatApp_Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Client_Process client;
        public MainWindow()
        {
            InitializeComponent();
            client = new Client_Process();
            
        }

        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            client.Send("Client" + client.ID.ToString() + " " + tbMessage.Text);
            tbMessage.Clear();
            tbMessage.Focus();
        }

        private void tbMessage_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                // Duplicate btnSend_Click()
                client.Send("Client" + client.ID.ToString() + " " + tbMessage.Text);
                tbMessage.Clear();
                tbMessage.Focus();
            }
        }
    }
}
