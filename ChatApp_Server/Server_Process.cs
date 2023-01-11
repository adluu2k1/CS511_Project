using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp_Server
{
    internal class Server_Process
    {
        // private fields and properties

        private TcpListener Listener;
        List<ClientConnection> ClientsList = new();


        // public fields and properties




        // private methods

        private void AcceptClient(IAsyncResult ar)
        {
            Console.WriteLine("Info: Incomming client...");
            try
            {
                ClientConnection client = new(Listener.EndAcceptTcpClient(ar));

                ClientsList.Add(client);
                Listener.BeginAcceptTcpClient(new AsyncCallback(AcceptClient), Listener);

                Console.WriteLine("Info: Client " + client.ID.ToString() + " joined the conversation.");
                SendAll("res 0 joined " + client.ID.ToString());

            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR: Cannot accept new client.");
                Debug.Print("\n" + e.ToString() + "\n");
            }
        }



        // public methods

        public Server_Process()
        {
            Listener = new TcpListener(IPAddress.Any, 1704);
        }

        public void Start()
        {
            try
            {
                Listener.Start();
                Console.WriteLine("Info: Server started.");
                Listener.BeginAcceptTcpClient(new AsyncCallback(AcceptClient), Listener);
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR: Cannot start server.");
                Debug.Print("\n" + e.ToString() + "\n");
            }
        }

        public void OnMessageReceived(ClientConnection client, string msg)
        {
            Console.WriteLine(msg);
            SendAll(msg);
        }

        public void SendAll(string str)
        {
            foreach (ClientConnection client in ClientsList)
            {
                client.Send(str);
            }
        }

        public void DisconnectClient(ClientConnection client)
        {
            ClientsList.Remove(client);
            Console.WriteLine("Info: Client " + client.ID.ToString() + " has been disconnected.");
            //SendAll("text 0 Client " + client.ID.ToString() + " has been disconnected.");
        }
    }

    internal class ClientConnection
    {
        public int ID { get; private set; } = 0;
        private TcpClient tcpClient;

        public ClientConnection(TcpClient tcpClient)
        {
            this.tcpClient = tcpClient;
            Console.WriteLine("Info: Setting client ID...");
            while (true)
            {
                string? msg = new StreamReader(tcpClient.GetStream()).ReadLine();
                if (msg != null)
                {
                    if (msg.StartsWith("SetID"))
                    {
                        ID = Convert.ToInt32(msg.Split(' ')[1]);
                    }
                    else
                    {
                        ID = new Random().Next(1, short.MaxValue);
                        Console.WriteLine("WARNING: Cannot retrieve ID from incomming client. A random ID has been assigned.");
                    }
                    break;
                }
            }

            this.tcpClient.GetStream().BeginRead(new byte[0], 0, 0, new AsyncCallback(CallBack_Read), null);
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
                Program.server.DisconnectClient(this);
                Debug.Print("\n" + e.ToString() + "\n");
            }
        }

        private void OnMessageReceived(string msg)
        {
            Program.server.OnMessageReceived(this, msg);
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
                Console.WriteLine("ERROR: Client " + ID + " cannot send message.");
                Debug.Print("\n" + e.ToString() + "\n");
            }
        }
    }
}
