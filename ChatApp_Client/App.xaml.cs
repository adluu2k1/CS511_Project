using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace ChatApp_Client
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static List<ClientData> clients = new();
        public static List<GroupData> groups = new();

        public static int CurrentClientID = -1;

        public static ChatApp_Client.MainWindow? mainWindow;

        public static int GetClientID(string username)
        {
            foreach (var client in clients)
            {
                if (client.Username == username)
                {
                    return client.ID;
                }
            }
            return -1;
        }

        public static string GetClientDisplayName(int id)
        {
            foreach (var client in clients)
            {
                if (client.ID == id)
                {
                    return client.DisplayName;
                }
            }
            return "null";
        }

        public static ClientData CreateClientData(int id, string username, string pass, string displayname, string ava_path)
        {
            ClientData client = new();
            client.ID = id;
            client.Username = username;
            client.Pass = pass;
            client.DisplayName = displayname;
            client.AvatarPath = ava_path;

            return client;
        }
        public static GroupData CreateGroupData(int iD, string displayName, string clientsList)
        {
            GroupData group = new();
            group.ID = iD;
            group.DisplayName = displayName;
            group.ClientsList = clientsList;

            return group;
        }

        public static void SaveClients()
        {
            File.WriteAllText("data/Clients.json", JsonSerializer.Serialize(clients));
        }
        public static void SaveGroups()
        {
            File.WriteAllText("data/Groups.json", JsonSerializer.Serialize(groups));
        }
    }
}
