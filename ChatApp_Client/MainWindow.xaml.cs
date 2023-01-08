using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Media.Media3D;
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
            Width = SystemParameters.MaximizedPrimaryScreenWidth * (3.0 / 5);
            Height = SystemParameters.MaximizedPrimaryScreenHeight * (4.0 / 5);

            client = new();
            
        }

        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            Send_tbMessageText();
        }

        private void tbMessage_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && tbMessage.Text != "")
            {
                Send_tbMessageText();
            }
        }

        private void tbMessage_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tbMessage.Text != "")
            {
                btnSend.IsEnabled = true;
            }
            else
            {
                btnSend.IsEnabled = false;
            }
        }

        private void Send_tbMessageText()
        {
            client.Send("Client" + client.ID.ToString() + " " + tbMessage.Text);
            tbMessage.Clear();
            tbMessage.Focus();
        }

        private void btnImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlgOpen = new();
            dlgOpen.RestoreDirectory = true;

            if (dlgOpen.ShowDialog() == true)
            {
                
            }
        }
    }
}
