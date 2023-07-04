using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace Server.Models
{
    public class ChatClientModel
    {
        public ChatClientModel(ChatListenerModel listenerModel, TcpClient client)
        {
            chatListener = listenerModel;

            _client = client;

            stream = _client.GetStream();

            reader = new StreamReader(stream);

            writer = new StreamWriter(stream);
        }

        private ChatListenerModel chatListener;
        private TcpClient _client;
        private NetworkStream stream;
        private StreamReader reader;
        public StreamWriter writer;

        public string UserName { get; set; }

        public async Task BeginProcessingUserAsync()
        {
            try
            {
                await writer.WriteLineAsync("Hello dear user, send me your name: ");

                await writer.FlushAsync();

                UserName = await reader.ReadLineAsync();

                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine($"User: {UserName} connected to the chat");

                await writer.WriteLineAsync($"Dear {UserName}, you connected to chat: {chatListener.ChatName}");

                await writer.FlushAsync();

                while (true)
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

                    foreach (ChatClientModel client in chatListener.clientModels.Where(c=>c.UserName != UserName))
                    {
                        await client.writer.WriteLineAsync($"{UserName}: " + message);
                        await client.writer.FlushAsync();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Connection is broken");
            }
        }
    }
}
