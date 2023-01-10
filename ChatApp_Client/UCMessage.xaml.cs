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
    /// Interaction logic for UCMessage.xaml
    /// </summary>
    public partial class UCMessage : UserControl
    {
        public UCMessage(Message msg, string avatar_path, int clientID, Brush background)
        {
            InitializeComponent();

            MsgTextBox.Text = msg.Content;

            AvatarWrapper.Background = new ImageBrush(new BitmapImage(new Uri(avatar_path, UriKind.RelativeOrAbsolute)));
            AvatarWrapper.CornerRadius = new CornerRadius(AvatarWrapper.Height / 2);

            SenderNameBlock.Text = msg.SenderName;

            if (msg.SenderID == 0)
            {
                this.HorizontalAlignment = HorizontalAlignment.Center;
                MsgTextBox.Foreground = Brushes.Gray;
            }
            else
            {
                ChatBubble.Background = background;
                AvatarWrapper.Visibility = Visibility.Visible;
                SenderNameBlock.Visibility = Visibility.Visible;

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
    }
}
