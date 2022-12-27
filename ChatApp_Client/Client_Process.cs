using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Threading;

namespace ChatApp_Client
{
    internal class Client_Process
    {
        public int ID { get; private set; } = new Random().Next(1, short.MaxValue);
        private TcpClient tcpClient = new("127.0.0.1", 1704);

        public Client_Process()
        {
            try
            {
                Send("SetID " + ID.ToString());
                NetworkStream stream = tcpClient.GetStream();
                stream.BeginRead(new byte[0], 0, 0, new AsyncCallback(CallBack_Read), null);

                MessageBox.Show("Connected to host successfully!");
            }
            catch (Exception e)
            {
                MessageBox.Show("Error connecting to host!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Debug.Print("\n" + e.ToString() + "\n");
            }

            Application.Current.MainWindow.Title = "Client ID: " + ID.ToString();
        }

        private void CallBack_Read(IAsyncResult ar)
        {
            try
            {
                string? msg = new StreamReader(tcpClient.GetStream()).ReadLine();
                if (msg != null)
                {
                    OnMessageReceived(msg);
                }

                tcpClient.GetStream().BeginRead(new byte[0], 0, 0, new AsyncCallback(CallBack_Read), null);
            }
            catch (Exception e)
            {
                MessageBox.Show("Error receiving message from host!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Debug.Print("\n" + e.ToString() + "\n");
            }
        }
        private void OnMessageReceived(string msg)
        {
            Application.Current.Dispatcher.Invoke(DisplayMessage, msg);
        }

        private void DisplayMessage(string msg)
        {
            RichTextBox rtbConsole = (RichTextBox)Application.Current.MainWindow.FindName("rtbConsole");

            Paragraph paragraph = new();
            paragraph.Inlines.Add(msg + "\n");
            if (msg.StartsWith("Client " + ID.ToString()))
            {
                paragraph.TextAlignment = TextAlignment.Right;
            }
            else if (msg.StartsWith("I:"))
            {
                paragraph.TextAlignment = TextAlignment.Center;
            }
            else
            {
                paragraph.TextAlignment = TextAlignment.Left;
            }
            rtbConsole.Document.Blocks.Add(paragraph);
            
        }
        public void Send(string str)
        {
            try
            {
                StreamWriter sWriter = new(tcpClient.GetStream());
                sWriter.WriteLine(str);
                sWriter.Flush();
            }
            catch (Exception e)
            {
                MessageBox.Show("Error sending message to host!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Debug.Print("\n" + e.ToString() + "\n");
            }
        }
    }
}
