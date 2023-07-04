using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace TcpServer
{
    class Program
    {
        static void Main(string[] args)
        {
            IPHostEntry hostEntry = Dns.GetHostEntry("localhost");
            IPAddress ipAddress = hostEntry.AddressList[0];
            IPEndPoint endPoint = new IPEndPoint(ipAddress, 5000);

            Socket listener = new Socket(ipAddress.AddressFamily,SocketType.Stream, ProtocolType.Tcp);

            try
            {
                listener.Bind(endPoint);
                listener.Listen(10);
                string request = null;

                while (true)
                {
                    Console.WriteLine($"waiting connection on port: {endPoint}");

                    Socket handler = listener.Accept();

                    byte[] reqBytes = new byte[1024];
                    handler.Receive(reqBytes);

                    request += Encoding.UTF8.GetString(reqBytes);
                    Console.WriteLine($"server receive message: {request}");

                    string answer = $"{request}";
                    handler.Send(Encoding.UTF8.GetBytes(answer));

                    if (request.IndexOf("<TheEnd>") > -1)
                    {
                        Console.WriteLine("Server finish connection with client");
                        break;
                    }

                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }

            Console.ReadLine();
        }
    }
}
