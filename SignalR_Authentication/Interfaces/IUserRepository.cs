using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SignalR_Authentication.Models;

namespace SignalR_Authentication.Interfaces
{
    public interface IUserRepository
    {

        public List<User> GetUsers();

        public User GetUserById(long id);

        public User GetUserByEmail(string Email);

        public Dialog FindDialog(long currentUserId, long receiverId);

        public string CreateDialog(long currentUserId, long receiverId);

        public void AddMessage(string dialogName, long currentUserId, string message);

        public List<Message> ShowAllMessages(string dialogName);

    }
}
