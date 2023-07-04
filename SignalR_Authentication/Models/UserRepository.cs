using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SignalR_Authentication.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace SignalR_Authentication.Models
{
    public class UserRepository : IUserRepository
    {
        private DataContext context;

        public UserRepository(DataContext context)
        {
            this.context = context;
        }

        public List<User> GetUsers()
        {
            return context.Users.ToList();
        }

        public User GetUserById(long id)
        {
            return context.Users.First(u=>u.Id == id);
        }

        public User GetUserByEmail(string email)
        {
            return context.Users.First(u=>u.Email == email);
        }

        public Dialog FindDialog(long currentUserId, long receiverId)
        {
            string currentEmail = GetUserById(currentUserId).Email;
            string receiverEmail  = GetUserById(receiverId).Email;

            Dialog dialog;

            try
            {
                dialog = context.Dialogs
                .Where(d => d.ChatName == $"{currentEmail}_{receiverEmail}" || d.ChatName == $"{receiverEmail}_{currentEmail}")
                .First();
                Console.WriteLine("Dialog founded!!!");
            }
            catch
            {
                dialog = null;
            }

            return dialog;
        }

        public string CreateDialog(long currentUserId, long receiverId)
        {
            User current = context.Users.First(u => u.Id == currentUserId);

            User receiver = context.Users.First(u => u.Id == receiverId);

            Dialog dialog = new Dialog();
            dialog.ChatName = $"{current.Email}_{receiver.Email}";

            context.Dialogs.Add(dialog);

            context.SaveChanges();

            Console.WriteLine("Dialog created "+dialog.ChatName);

            return dialog.ChatName;
        }

        public void AddMessage(string dialogName, long id, string message)
        {
            User user = GetUserById(id);

            Dialog dialog = context.Dialogs.First(d => d.ChatName == dialogName);

            context.Messages.Add(new Message() { Text = message, UserName = user.Name, Dialog = dialog });

            context.SaveChanges();
        }

        public List<Message> ShowAllMessages(string dialogName)
        {
            List<Message> messages;

            try
            {
                Dialog dialog = context.Dialogs.Include(d => d.Messages).First(d => d.ChatName == dialogName);

                foreach (Message m in dialog.Messages)
                {
                    m.Dialog = null;
                }

                messages = dialog.Messages;
            }
            catch
            {
                messages = null;
            }

            return messages;
        }
    }
}
