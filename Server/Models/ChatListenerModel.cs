using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace Server.Models
{
    public class ChatListenerModel
    {
        public ChatListenerModel(IPAddress address, int port, string chatName)
        {
            listener = new TcpListener(address, port);

            ChatName = chatName;

            clientModels = new List<ChatClientModel>();
        }

        public string ChatName { get; set; }
        private TcpListener listener;
        public List<ChatClientModel> clientModels;

        public async Task BeginClientsAsseptionAsync()
        {
            listener.Start();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Server: {(string.IsNullOrEmpty(ChatName) ? listener.LocalEndpoint : ChatName)} start working");

            try
            {
                while (true)
                {
                    TcpClient client = await listener.AcceptTcpClientAsync();

                    ChatClientModel chatClient = new ChatClientModel(this, client);

                    clientModels.Add(chatClient);

                    Task.Run(async () => await chatClient.BeginProcessingUserAsync()); // write smth
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Server stop working");
                listener.Stop();
            }
        }

    }
}
