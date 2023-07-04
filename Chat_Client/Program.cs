using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Text;
using System.Collections.Generic;
using System.IO;

namespace Chat_Client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            TcpClient client = new TcpClient(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5000));
            await client.ConnectAsync(IPAddress.Parse("127.0.0.1"), 8888);

            var stream = client.GetStream();
            var reader = new StreamReader(stream);
            var writer = new StreamWriter(stream);


            string nameAsk = await reader.ReadLineAsync();
            Console.Write(nameAsk+": ");
            string name = Console.ReadLine();
            await writer.WriteLineAsync(name);
            await writer.FlushAsync();

            

            Task.Run(async() =>
            {
                await Task.Delay(1000);
                while (true)
                {
                    string mes = Console.ReadLine();
                    await writer.WriteLineAsync(mes);
                    await writer.FlushAsync();
                }
            });

            while (true)
            {
                try
                {
                    string message = await reader.ReadLineAsync();
                    if (string.IsNullOrEmpty(message))
                    {
                        continue;
                    }
                    Console.WriteLine(message);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Dermo");
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}
