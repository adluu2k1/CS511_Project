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
        public GroupData group;

        public Message? LatestMessage;
        public Window? MsgBox_AddPlaylist;

        public MainWindow(Client_Process client)
        {
            InitializeComponent();
            Width = SystemParameters.MaximizedPrimaryScreenWidth * (2.5 / 5);
            Height = SystemParameters.MaximizedPrimaryScreenHeight * (4.0 / 5);

            this.client = client;
            this.group = App.CreateGroupData("", client.ID.ToString());

            tbGroupName.Text = group.DisplayName;
            btnGroupRename.Visibility = Visibility.Visible;
        }

        public MainWindow(Client_Process client, int groupID)
        {
            InitializeComponent();
            Width = SystemParameters.MaximizedPrimaryScreenWidth * (2.5 / 5);
            Height = SystemParameters.MaximizedPrimaryScreenHeight * (4.0 / 5);

            this.client = client;
            this.group = App.CreateGroupData("", client.ID.ToString());

            tbGroupName.Text = "Group ID: " + groupID.ToString();
        }

        public void DisplayMessage(Message msg, string avatar_path)
        {
            if (client == null)
            {
                return;
            }

            UCMessage MessageUI;
            if (LatestMessage == null || msg.SenderID != LatestMessage.SenderID)
            {
                MessageUI = new(msg, avatar_path, client.ID, menuMore.Background, false);
            }
            else
            {
                MessageUI = new(msg, avatar_path, client.ID, menuMore.Background, true);
            }
            MessageUI.ToolTip = msg.TimeSent.ToString();

            stackChatHistory.Children.Add(MessageUI);

            scrollChatHistory.ScrollToEnd();

            LatestMessage = msg;
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
            string msg = "text " + client.ID.ToString() + " " + tbMessage.Text;
            client.Send(msg);
            tbMessage.Clear();
            tbMessage.Focus();
        }

        private void btnImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlgOpen = new();
            dlgOpen.RestoreDirectory = true;

            if (dlgOpen.ShowDialog() == true)
            {
                string msg = "image " + client.ID.ToString() + " " + dlgOpen.FileName;
                client.Send(msg);
                tbMessage.Focus();
            }
        }

        private void btnAudio_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlgOpen = new();
            dlgOpen.RestoreDirectory = true;

            if (dlgOpen.ShowDialog() == true)
            {
                string msg = "media " + client.ID.ToString() + " " + dlgOpen.FileName;
                client.Send(msg);
                tbMessage.Focus();
            }
        }

        private void btnVideo_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlgOpen = new();
            dlgOpen.RestoreDirectory = true;

            if (dlgOpen.ShowDialog() == true)
            {
                string msg = "media " + client.ID.ToString() + " " + dlgOpen.FileName;
                client.Send(msg);
                tbMessage.Focus();
            }
        }

        private void btnGroupRename_Click(object sender, RoutedEventArgs e)
        {
            MsgBox_AddPlaylist = new Window();
            MsgBox_AddPlaylist.Width = 300;
            MsgBox_AddPlaylist.Height = 150;
            MsgBox_AddPlaylist.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            MsgBox_AddPlaylist.Background = new SolidColorBrush(Color.FromRgb(52, 52, 52));
            MsgBox_AddPlaylist.Title = "Rename Group";

            Label labelMsg = new Label();
            labelMsg.Content = "Group Name: ";
            labelMsg.Margin = new Thickness(20, 30, 0, 0);

            TextBox txtboxPlaylistName = new TextBox();
            txtboxPlaylistName.Margin = new Thickness(40, 30, 0, 0);
            txtboxPlaylistName.Width = 130;
            txtboxPlaylistName.Foreground = Brushes.White;
            txtboxPlaylistName.Name = "txtboxPlaylistName";

            Button btnAdd = new Button();
            btnAdd.Content = "Rename";
            btnAdd.Margin = new Thickness(30, 50, 30, 0);

            btnAdd.Click += new RoutedEventHandler(btnOk_Click);

            Grid grid = new();

            grid.Children.Add(labelMsg);
            grid.Children.Add(txtboxPlaylistName);
            grid.Children.Add(btnAdd);

            MsgBox_AddPlaylist.Content = grid;

            if (MsgBox_AddPlaylist.ShowDialog() == true)
            {
                tbGroupName.Text = txtboxPlaylistName.Text + " (Group ID: " + group.ID.ToString() + ")";
                client.Send("res 0 grouprename " + txtboxPlaylistName.Text);
            }

        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            MsgBox_AddPlaylist!.DialogResult = true;
        }
    }
}
