using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace SimpleChat.Models
{
    public class ChatHub : Hub
    {
        public async Task Send(string message, string userName)
        {
            await Clients.Others.SendAsync("Receive", message, userName);
        }


        public override async Task OnConnectedAsync()
        {
            await Clients.Caller.SendAsync("Notify", "you are connected!");
            Console.WriteLine("User connected");
            Console.WriteLine("1. "+Context.ConnectionId);
            Console.WriteLine("2. " + Context.UserIdentifier);
            Console.WriteLine("3. " + Context.User.Identity.IsAuthenticated);
            Console.WriteLine("4. " + Context.GetHttpContext().User.Identity.IsAuthenticated);
            Console.WriteLine($"user ip address: " +
                $"{Context.GetHttpContext().Connection.LocalIpAddress.ToString()}:{Context.GetHttpContext().Connection.LocalPort}");
            Console.WriteLine($"server ip address: " +
                $"{Context.GetHttpContext().Connection.RemoteIpAddress.ToString()}:{Context.GetHttpContext().Connection.RemotePort}");
            Console.WriteLine($"user-agent: {Context.GetHttpContext().Request.Headers["User-Agent"]}");

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            Console.WriteLine("User disconnected");
            await base.OnDisconnectedAsync(exception);
        }
    }
}
