using System;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TCP_Client_Two
{
    class Program
    {
        static async Task Main(string[] args)
        {
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5200);
            TcpClient client = new TcpClient(endPoint);
            Console.WriteLine($"Client: {client.Client.LocalEndPoint} start working!");

            try
            {
                await client.ConnectAsync(IPAddress.Parse("127.0.0.1"), 5000);
                Console.WriteLine($"Client connected to: {client.Client.RemoteEndPoint}");

                var stream = client.GetStream();

                StringBuilder receiver = new StringBuilder();

                while (true)
                {
                    Console.Write("Write message to server: ");
                    receiver.Append(Console.ReadLine());
                    receiver.Append("\n");

                    byte[] data = Encoding.ASCII.GetBytes(receiver.ToString());
                    await stream.WriteAsync(data);


                    if (receiver.ToString().ToLower().Contains("end"))
                    {
                        break;
                    }

                    receiver.Clear();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                client.Close();
            }
        }
    }
}
