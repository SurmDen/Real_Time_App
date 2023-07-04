using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TCP_Duplex_Listener
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Random random = new Random();

            

            Dictionary<string, string> dictionary = new Dictionary<string, string>()
            {
                {"cat", "кот - сурман" },
                {"rat", "мышь - слусарев" },
                {"dog", "собака - шушканов" }
            };

            TcpListener listener = new TcpListener(IPAddress.Parse("127.0.0.1" ),1000);
            try
            {
                listener.Start();
                Console.WriteLine("Server translater launched!");

                while (true)
                {
                    TcpClient client = await listener.AcceptTcpClientAsync();
                    var stream = client.GetStream();
                    Console.WriteLine($"Server accepted client: {client.Client.RemoteEndPoint}");

                    int symbol = '\n';
                    List<byte> bytes = new List<byte>();
                    StringBuilder answer = new StringBuilder();

                    while (true)
                    {
                        while ((symbol = stream.ReadByte()) != '\n')
                        {
                            bytes.Add((byte)symbol);
                        }

                        string word = Encoding.UTF8.GetString(bytes.ToArray());
                        Console.WriteLine($"client asked to translate word: {word}");
                        

                        try
                        {
                            answer.Append(dictionary[word]);
                        }
                        catch
                        {
                            answer.Append("Dictionary not contains translation of this word");
                        }
                        finally
                        {
                            answer.Append('\n');
                        }

                        await stream.WriteAsync(Encoding.UTF8.GetBytes(answer.ToString())) ;

                        bytes.Clear();
                        answer.Clear();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                listener.Stop();
            }
        }
    }
}
