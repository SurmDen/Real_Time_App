using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace Chat_Server.Tools
{
    public class TcpListenerModel
    {
        private TcpListener listener;
        public List<TcpClientModel> Clients;

        public TcpListenerModel(IPAddress address, int port, string name = null)
        {
            listener = new TcpListener(address, port);
           
            ServerName = name;
            
            Clients = new List<TcpClientModel>();
        }

        public string ServerName { get; set; }

        public async Task BeginAccertionAsync()
        {
            listener.Start();
            Console.WriteLine($"Server: {(ServerName == null ? listener.LocalEndpoint : ServerName)} start working");

            while (true)
            {
                try
                {
                    TcpClient client = await listener.AcceptTcpClientAsync();
                    TcpClientModel clientModel = new TcpClientModel(this, client);
                    Clients.Add(clientModel);

                    Task.Run(async () => await clientModel.ConnectToServerAsync());
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                
            }

        }

        public bool Disconnect(string clientName)
        {
            try
            {
                TcpClientModel tcpClient = Clients.First(c => c.Name == clientName);
                tcpClient.client.Client.Close();
            }
            catch
            {
                return false;
            }

            return true;
        }

        public void DisconnectAll()
        {
            foreach (var client in Clients)
            {
                client.client.Close();
            }
        }
    }
}
