using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Second_Tcp_Client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            TcpClient client = new TcpClient(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 6000));
            Console.WriteLine($"client: {client.Client.LocalEndPoint} start working");
            try
            {
                await client.ConnectAsync(IPAddress.Parse("127.0.0.1"), 8888);
                var stream = client.GetStream();

                Console.WriteLine($"Connection to server: {client.Client.RemoteEndPoint} established");

                List<byte> bytes = new List<byte>();
                int symbol = '\n';
                StringBuilder builder = new StringBuilder();

                while (true)
                {
                    Console.Write("Write message to server: ");
                    Console.ForegroundColor = ConsoleColor.Green;

                    builder.Append(Console.ReadLine());
                    builder.Append('\n');

                    Console.ForegroundColor = ConsoleColor.White;

                    await stream.WriteAsync(Encoding.UTF8.GetBytes(builder.ToString()));

                    while ((symbol = stream.ReadByte()) != '\n')
                    {
                        bytes.Add((byte)symbol);
                    }

                    Console.WriteLine($"form server: {Encoding.UTF8.GetString(bytes.ToArray())}");

                    builder.Clear();
                }

            }
            catch (Exception e)
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
