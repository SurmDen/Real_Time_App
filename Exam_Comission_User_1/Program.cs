using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace Exam_Comission_User_1
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Green;

            TcpClient client = new TcpClient(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 10000));
            await client.ConnectAsync(IPAddress.Parse("127.0.0.1"), 8888);

            var stream = client.GetStream();
            var reader = new StreamReader(stream);
            var writer = new StreamWriter(stream);

            Console.Write(await reader.ReadLineAsync());

            await writer.WriteLineAsync(Console.ReadLine());
            await writer.FlushAsync();

            Console.WriteLine(await reader.ReadLineAsync());

            Console.ForegroundColor = ConsoleColor.DarkMagenta;

            Task.Run(async () =>
            {
                try
                {
                    while (true)
                    {
                        string message = Console.ReadLine();

                        await writer.WriteLineAsync(message);
                        await writer.FlushAsync();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    Console.WriteLine("Client 1 write error");
                }
                
            });

            await Task.Run(async () =>
            {
                try
                {
                    while (true)
                    {
                        string message = await reader.ReadLineAsync();

                        if (string.IsNullOrEmpty(message))
                        {
                            continue;
                        }

                        Console.WriteLine(message);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    Console.WriteLine("Client 1 read error");
                }
            });


        }
    }
}
