using System;
using System.Net;
using System.Net.Sockets;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text;
using System.Collections.Generic;

namespace TCP_Listener
{
    class Program
    {
        static async Task Main(string[] args)
        {
            TcpListener tcpListener = new TcpListener(IPAddress.Parse("127.0.0.1"), 5000);
           
            try
            {
                tcpListener.Start();
                Console.WriteLine($"Server: {tcpListener.LocalEndpoint} launched!");

                while (true)
                {
                    TcpClient client = await tcpListener.AcceptTcpClientAsync();
                    Console.WriteLine($"{client.Client.RemoteEndPoint} accepted");
                    var stream = client.GetStream();

                    List<byte> storage = new List<byte>();
                    int symbol = '\n';

                    while (true)
                    {
                        while ((symbol = stream.ReadByte()) != '\n')
                        {
                            storage.Add((byte)symbol);
                        }

                        string message = Encoding.ASCII.GetString(storage.ToArray());

                        Console.WriteLine($"received message: {message}");

                        if (message.ToLower().Contains("end"))
                        {
                            Console.WriteLine("Client stopped connection!");
                            break;
                        }

                        storage.Clear();
                        
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                tcpListener.Stop();
            }
        }
    }
}
