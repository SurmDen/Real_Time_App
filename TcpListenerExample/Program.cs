using System;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace TcpListenerExample
{
    class Program
    {
        static async Task Main(string[] args)
        {
            TcpListener listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 6000);
            listener.Start();
            Console.WriteLine("Server launching...");

            while (true)
            {
                Socket socket = await listener.AcceptSocketAsync();

                if (socket.Connected)
                {
                    Console.WriteLine($"socket connected to: {socket.RemoteEndPoint}");
                }

                byte[] receiveBytes = new byte[1024];
                
                socket.Receive(receiveBytes);

                string message = Encoding.UTF8.GetString(receiveBytes);

                socket.Send(Encoding.UTF8.GetBytes("server answered: "+message));


                if (message.ToLower().Contains("stop"))
                {
                    Console.WriteLine("Client stopped connection");
                    break;
                }


                socket.Shutdown(SocketShutdown.Both);
                socket.Close();

            }

            listener.Stop();
        }
    }
}
