using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TCP_Duplex_Client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Random random = new Random();

           

            IPEndPoint endPoint = new(IPAddress.Parse("127.0.0.1"), 1100);
            TcpClient client = new TcpClient(endPoint);

            try
            {
                await client.ConnectAsync(IPAddress.Parse("127.0.0.1"), 1000);
                Console.WriteLine($"client connected to server: {client.Client.RemoteEndPoint}");

                var stream = client.GetStream();

                int symb = '\n';
                List<byte> bytes = new List<byte>();
                StringBuilder builder = new StringBuilder();

                while (true)
                {
                    Console.Write("Insert word to translate: ");
                    builder.Append(Console.ReadLine());
                    builder.Append('\n');

                    await stream.WriteAsync(Encoding.UTF8.GetBytes(builder.ToString()));

                    while ((symb = stream.ReadByte()) != '\n')
                    {
                        bytes.Add((byte)symb);
                    }

                    string message = Encoding.UTF8.GetString(bytes.ToArray());
                    Console.WriteLine($"server answered: {message}");

                    bytes.Clear();
                    builder.Clear();
                }
            }
            catch (Exception e )
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                client.Close();
            }
        }
    }
}
