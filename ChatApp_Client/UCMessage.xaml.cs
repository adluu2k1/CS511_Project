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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ChatApp_Client
{
    /// <summary>
    /// Interaction logic for UCMessage.xaml
    /// </summary>
    public partial class UCMessage : UserControl
    {
        public UCMessage(Message msg, string avatar_path, int clientID, Brush background, bool IsSameSender)
        {
            InitializeComponent();

            MsgTextBox.Text = msg.Content;

            AvatarWrapper.Background = new ImageBrush(new BitmapImage(new Uri(avatar_path, UriKind.RelativeOrAbsolute)));
            AvatarWrapper.CornerRadius = new CornerRadius(AvatarWrapper.Height / 2);

            SenderNameBlock.Text = App.GetClientDisplayName(msg.SenderID);

            if (msg.SenderID == 0)
            {
                this.HorizontalAlignment = HorizontalAlignment.Center;
                MsgTextBox.Foreground = Brushes.Gray;
                btnDownload.Visibility = Visibility.Collapsed;
            }
            else
            {
                switch (msg.Type)
                {
                    case Message.MessageType.Text:
                        btnDownload.Visibility = Visibility.Collapsed;
                        ChatBubble.Background = background;
                        break;

                    case Message.MessageType.Image:
                        MsgTextBox.Visibility = Visibility.Collapsed;

                        image.Source = new BitmapImage(new Uri(msg.Content, UriKind.Absolute));
                        image.Visibility = Visibility.Visible;

                        break;

                    case Message.MessageType.Media:
                        MsgTextBox.Visibility = Visibility.Collapsed;

                        media.Source = new(msg.Content);
                        media.Pause();
                        panelMediaUI.Visibility = Visibility.Visible;

                        break;
                }
                


                if (!IsSameSender)
                {
                    AvatarWrapper.Visibility = Visibility.Visible;
                    SenderNameBlock.Visibility = Visibility.Visible;
                }
                else
                {
                    AvatarWrapper.Visibility = Visibility.Hidden;
                }

                if (msg.SenderID == clientID)
                {
                    this.HorizontalAlignment = HorizontalAlignment.Right;
                    this.FlowDirection = FlowDirection.RightToLeft;
                }
                else
                {
                    this.HorizontalAlignment = HorizontalAlignment.Left;
                    this.FlowDirection = FlowDirection.LeftToRight;
                }
            }
        }

        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            media.Play();
        }

        private void btnPause_Click(object sender, RoutedEventArgs e)
        {
            media.Pause();
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            media.Stop();
        }

        private void btnDownload_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dlgDownload = new SaveFileDialog();
            dlgDownload.RestoreDirectory = true;
            dlgDownload.Title = "Download";

            if (dlgDownload.ShowDialog() == true)
            {
                File.Copy(MsgTextBox.Text, dlgDownload.FileName, true);
                MessageBox.Show("Đã tải xuống thành công!");
            }
        }
    }
}
