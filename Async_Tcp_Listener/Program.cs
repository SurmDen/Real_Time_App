using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Async_Tcp_Listener
{
    class Program
    {
        static async Task Main(string[] args)
        {
            TcpListener listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 8888);
            Console.WriteLine("Server starts working...");
            listener.Start();

            try
            {
                while (true)
                {
                    TcpClient client = await listener.AcceptTcpClientAsync();
                    Console.WriteLine($"client: {client.Client.RemoteEndPoint} connected");

                    //object ob = new object();

                    //lock (ob)
                    //{
                        Task.Run(async () => await ProcessAcceptedClient(client));
                    //}
                }
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            finally
            {
                listener.Stop();
            }
        }

        static async Task ProcessAcceptedClient(TcpClient client)
        {
            var stream = client.GetStream();

            List<byte> bytes = new List<byte>();
            int symbol = '\n';

            while (true)
            {
                while ((symbol = stream.ReadByte()) != '\n')
                {
                    bytes.Add((byte)symbol);
                }

                string message = Encoding.UTF8.GetString(bytes.ToArray());

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"from {client.Client.RemoteEndPoint}: {message}");
                Console.ForegroundColor = ConsoleColor.White;

                if (message.ToLower() == "end")
                {
                    Console.ForegroundColor = ConsoleColor.Red;

                    Console.WriteLine($"Cliend: {client.Client.RemoteEndPoint} stopped connection");

                    Console.ForegroundColor = ConsoleColor.Red;

                    break;
                }

                await stream.WriteAsync(Encoding.UTF8.GetBytes("Your message successfully received\n"));
                bytes.Clear();
            }

            client.Close();
        }
    }
}
