using System;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace TcpClientExample
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //we are creating tcp client with local endpoint
            using TcpClient client = new TcpClient(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 6100));

            try
            {
                //connecting to tcp server
                await client.ConnectAsync(IPAddress.Parse("127.0.0.1"), 6000);
                if (client.Connected)
                {
                    Console.WriteLine($"client connected to remote server");
                }

                //getting stream to communicate with server
                NetworkStream stream = client.GetStream();

                //sending message to remote tcp server
                while (true)
                {
                    Console.Write("Write message to server: ");
                    string message = Console.ReadLine();
                    await stream.WriteAsync(Encoding.UTF8.GetBytes(message));

                    //getting answer
                    byte[] ansBytes = new byte[1024];
                    StringBuilder builder = new StringBuilder();
                    int bytesCount;

                    do
                    {
                        bytesCount = await stream.ReadAsync(ansBytes);

                        builder.Append(Encoding.UTF8.GetString(ansBytes, 0, bytesCount));

                    } while (bytesCount > 0);

                    Console.WriteLine(builder);

                    if (message.ToLower().Contains("stop"))
                    {
                        client.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Console.ReadLine();
            }
        }
    }
}
