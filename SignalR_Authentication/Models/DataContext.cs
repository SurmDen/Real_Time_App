using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SignalR_Authentication.Models;

namespace SignalR_Authentication.Models
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Dialog> Dialogs { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            User[] users = new User[]
            {
                new User{Id = 1, Name = "Denis", Email = "surm@den", Role = "Admin", Password = "denis123"},
                new User{Id = 2, Name = "Dasha", Email = "dan@gash", Role = "User", Password = "dasha123"},
                new User{Id = 3, Name = "Pasha", Email = "gub@pash", Role = "User", Password = "pasha123"},
            };

            builder.Entity<User>().HasData(users);
        }

    }
}
