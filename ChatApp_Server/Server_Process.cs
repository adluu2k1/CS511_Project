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
            ClientConnection client = new();
            
            ClientsList.Add(client);
            Console.WriteLine("New client joined.\n");

            Listener.BeginAcceptTcpClient(new AsyncCallback(AcceptClient), Listener);
        }



        // public methods

        public Server_Process()
        {
            Listener = new TcpListener(IPAddress.Any, 1704);
        }

        public void Start()
        {
            Listener.Start();
            Console.Write("Waiting for connection...\n");
            Listener.BeginAcceptTcpClient(new AsyncCallback(AcceptClient), Listener);
        }

        public void OnMessageReceived(ClientConnection client, string msg)
        {
            Console.WriteLine("Client " + client.ID.ToString() + ": " + msg);
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
            //rtxtboxLog_AppendText("Client exited.\n");
        }
    }

    internal class ClientConnection
    {
        public int ID { get; private set; }
        private TcpClient tcpClient = new();

        public ClientConnection()
        {
            ID = new Random().Next(1000);
            tcpClient.GetStream().BeginRead(new byte[0], 0, 0, new AsyncCallback(CallBack_Read), null);
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
                Debug.Print(e.ToString());
            }
        }

        private void OnMessageReceived(string msg)
        {
            Program.server.OnMessageReceived(this, msg);
        }

        public void Send(string str)
        {
            StreamWriter sWriter = new(tcpClient.GetStream());
            sWriter.WriteLine(str);
            sWriter.Flush();
        }
    }
}
