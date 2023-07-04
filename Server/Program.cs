using System;
using System.Threading.Tasks;
using Server.Models;
using System.Net;
using System.Net.Sockets;

namespace Server
{
    class Program
    {
        static async Task Main(string[] args)
        {
            ChatListenerModel tcpServer = new ChatListenerModel(IPAddress.Parse("127.0.0.1"), 8888, "Milf Hunters");
            await tcpServer.BeginClientsAsseptionAsync();
        }
    }
}
