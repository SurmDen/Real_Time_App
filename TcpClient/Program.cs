using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace TcpClient
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("First client");
                StartTctConection(5000);
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
        }

        static void StartTctConection(int port)
        {
            IPHostEntry entry = Dns.GetHostEntry("localhost");
            IPAddress address = entry.AddressList[0];
            IPEndPoint endPoint = new IPEndPoint(address, port);

            Socket client = new Socket(address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            client.Connect(endPoint);

            Console.WriteLine(client.LocalEndPoint);
            Console.Write("write message to server: ");
            string message = "from first client " + Console.ReadLine();
            client.Send(Encoding.UTF8.GetBytes(message));

            byte[] answerBytes = new byte[1024];
            Console.WriteLine($"{Encoding.UTF8.GetString(answerBytes, 0,  client.Receive(answerBytes))}");

            Console.WriteLine();

            if (message.IndexOf("<TheEnd>") == -1)
            {

                StartTctConection(port);
            }

            client.Shutdown(SocketShutdown.Both);
            client.Close();
        }
    }
}
