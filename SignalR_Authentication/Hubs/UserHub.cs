using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using SignalR_Authentication.Interfaces;
using SignalR_Authentication.Models;
using Microsoft.Extensions.Caching.Memory;

namespace SignalR_Authentication.Hubs
{
    [Authorize]
    public class UserHub : Hub
    {
        private IMemoryCache cache;
        private IUserRepository repository;
        User current;

        public UserHub(IUserRepository repository, IMemoryCache cache)
        {
            this.repository = repository;
            this.cache = cache;
        }

        public async Task Send(string message)
        {
            current = cache.Get<User>("currentUser");

            string name = Context.User.Identity.Name;
            await Clients.Group(cache.Get<string>("dialogName")).SendAsync("Receive", message, name);
            repository.AddMessage(cache.Get<string>("dialogName"), current.Id, message);
        }

        public async Task Init(string receiverId)
        {
            current = cache.Get<User>("currentUser");

            Console.WriteLine("Dialog start to initialized");

            long recId = long.Parse(receiverId);

            Dialog dialog;

            try
            {
                dialog = repository.FindDialog(current.Id, recId);
            }
            catch
            {
                dialog = null;
            }

            if (dialog == null)
            {
                string dialogName = repository.CreateDialog(current.Id, recId);
                cache.Set("dialogName", dialogName);
                await Groups.AddToGroupAsync(Context.ConnectionId, dialogName);

            }
            else
            {
                cache.Set("dialogName", dialog.ChatName);
                await Groups.AddToGroupAsync(Context.ConnectionId, dialog.ChatName);

                try
                {
                    foreach (Message message in repository.ShowAllMessages(dialog.ChatName))
                    {
                        await Clients.Caller.SendAsync("Receive", message.Text, message.UserName);
                    }
                }
                catch
                {
                    Console.WriteLine("Message list empty");
                }
            }

        }

        public override async Task OnConnectedAsync()
        {
            current = repository.GetUserByEmail(Context.User.FindFirst(ClaimTypes.Email).Value);

            cache.Set<User>("currentUser", current);

            Console.WriteLine($"User: {current.Email} connected");

            await base.OnConnectedAsync();
        }
    }
}
