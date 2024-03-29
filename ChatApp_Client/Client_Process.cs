﻿using System;
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
    public class Client_Process
    {
        public int ID { get; private set; }
        public string DisplayName { get; private set; }
        private TcpClient? tcpClient;

        //ChatApp_Client.MainWindow? winMain = Application.Current.Windows.OfType<ChatApp_Client.MainWindow>().FirstOrDefault();

        public Client_Process(int clientID)
        {
            if (clientID == -1)
            {
                ID = new Random().Next(1, short.MaxValue);
            }
            else
            {
                ID = clientID;
            }
            App.CurrentClientID = ID;

            DisplayName = App.GetClientDisplayName(ID);

            try
            {
                tcpClient = new("127.0.0.1", 1704);
                Send("SetID " + ID.ToString());
                NetworkStream stream = tcpClient.GetStream();
                stream.BeginRead(new byte[0], 0, 0, new AsyncCallback(CallBack_Read), null);
            }
            catch (Exception e)
            {
                MessageBox.Show("Error connecting to host!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Debug.Print("\n" + e.ToString() + "\n");
            }

        }

        private void CallBack_Read(IAsyncResult ar)
        {
            try
            {
                string? msg = new StreamReader(tcpClient!.GetStream()).ReadLine();
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
            string[] arr_msg = msg.Split(" ");
            int senderID = int.Parse(arr_msg[1]);
            string main_msg = String.Join(" ", arr_msg[2..]);

            Message.MessageType msg_type = Message.MessageType.Text;
            switch (arr_msg[0])
            {
                case "image":
                    msg_type = Message.MessageType.Image;
                    break;
                case "media":
                    msg_type = Message.MessageType.Media;
                    break;
                case "res":
                    msg_type = Message.MessageType.Response;
                    if (arr_msg[2] == "joined")
                    {
                        App.UpdateClients();
                        main_msg = App.GetClientDisplayName(int.Parse(arr_msg[3])) + " joined the conversation.";
                    }
                    else if (arr_msg[2] == "disconnected")
                    {
                        main_msg = App.GetClientDisplayName(int.Parse(arr_msg[3])) + " has been disconnected.";
                    }
                    else if (arr_msg[2] == "grouprename")
                    {
                        main_msg = "The group was renamed to " + String.Join(" ", arr_msg[3..]);
                        App.mainWindow!.Dispatcher.Invoke(SetGroupName, String.Join(" ", arr_msg[3..]));
                    }
                    break;
            }

            Message message = new(msg_type, senderID, main_msg, DateTime.Now);

            if (App.mainWindow != null)
            {
                App.mainWindow!.Dispatcher.Invoke(App.mainWindow!.DisplayMessage, message, App.GetClientAvatarPath(senderID));
            }
        }

        public void SetGroupName(string groupName)
        {
            App.mainWindow!.tbGroupName.Text = groupName + " (Group ID: " + App.mainWindow.tbGroupName.Text.Split(" ").Last().Replace(")", "") + ")";
        }

        public void Send(string str)
        {
            try
            {
                StreamWriter sWriter = new(tcpClient!.GetStream());
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
