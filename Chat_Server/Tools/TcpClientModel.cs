using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace Chat_Server.Tools
{
    public class TcpClientModel
    {
        

        public TcpClientModel(TcpListenerModel listener, TcpClient client)
        {
            this.listener = listener;
            this.client = client;
            
        }

        private TcpListenerModel listener;
        public TcpClient client;
        private NetworkStream stream;
        public StreamReader reader;
        public StreamWriter writer;

        public string Name { get; set; }

        public async Task ConnectToServerAsync()
        {
            stream = client.GetStream();
            reader = new StreamReader(stream);
            writer = new StreamWriter(stream);

            await writer.WriteLineAsync("Hi my dear user, send me you name please");
            await writer.FlushAsync();
            Name = await reader.ReadLineAsync();
            await writer.WriteLineAsync($"Thanks you, {Name}, your are connected to the chat: {listener.ServerName.ToUpper()}");
            await writer.FlushAsync();
            Console.WriteLine($"Client {Name} connected");

            while (true)
            {
                try
                {
                    string message = await reader.ReadLineAsync();
                    if (message.ToLower() == "end")
                    {
                        break;
                    }

                    if (string.IsNullOrEmpty(message))
                    {
                        continue;
                    }

                    Console.WriteLine(message);

                    foreach (TcpClientModel clientModel in listener.Clients.Where(c=>c.Name != this.Name))
                    {
                        await clientModel.writer.WriteLineAsync($"{Name}: {message}");
                        await clientModel.writer.FlushAsync();
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

            client.Close();
        }
    }
}
