using System;
using Chat_Server.Tools;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Chat_Server
{
    class Program
    {
        static async Task Main(string[] args)
        {
            TcpListenerModel tcpListener = new TcpListenerModel(IPAddress.Parse("127.0.0.1"), 8888, "SurmTwit");
            await tcpListener.BeginAccertionAsync();
        }
    }
}
