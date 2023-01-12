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

        public static void UpdateClients()
        {
            // Clients data

            string json_path = "data/Clients.json";
            if (!File.Exists(json_path))
            {
                return;
            }
            clients = JsonSerializer.Deserialize<List<ClientData>>(File.ReadAllText(json_path))!;
        }
        public static void UpdateGroups()
        {
            // Groups data

            string json_path = "data/Groups.json";
            if (!File.Exists(json_path))
            {
                return;
            }
            groups = JsonSerializer.Deserialize<List<GroupData>>(File.ReadAllText(json_path))!;
        }

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

        public static string GetClientAvatarPath(int id)
        {
            foreach (var client in clients)
            {
                if (client.ID == id)
                {
                    return client.AvatarPath;
                }
            }
            return "null";
        }

        public static string GetGroupDisplayName(int id)
        {
            foreach (var group in groups)
            {
                if (group.ID == id)
                {
                    return group.DisplayName;
                }
            }
            return "null";
        }

        public static bool IsClientIDTaken(int clientID)
        {
            foreach (var client in clients)
            {
                if (client.ID == clientID)
                {
                    return true;
                }
            }
            return false;
        }
        public static bool IsGroupIDTaken(int groupID)
        {
            foreach (var group in groups)
            {
                if (group.ID == groupID)
                {
                    return true;
                }
            }
            return false;
        }

        public static ClientData CreateClientData(string email, string username, string pass, string displayname, string ava_path)
        {
            ClientData client = new();
            do
            {
                client.ID = new Random().Next(1, short.MaxValue);
            } while (IsClientIDTaken(client.ID));
            client.Email = email;
            client.Username = username;
            client.Pass = pass;
            client.DisplayName = displayname;
            if (displayname == "")
            {
                client.DisplayName = username;
            }
            client.AvatarPath = ava_path;

            return client;
        }
        public static GroupData CreateGroupData(string displayName = "", string clientsList = "")
        {
            GroupData group = new();

            do
            {
                group.ID = new Random().Next(1, short.MaxValue);
            } while (IsGroupIDTaken(group.ID));

            if (displayName == "")
            {
                group.DisplayName = "Group ID: " + group.ID.ToString();
            }
            else
            {
                group.DisplayName = displayName;
            }

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
