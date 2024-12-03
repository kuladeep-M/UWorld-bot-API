using ChatHubSignalR.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatHubSignalR.Repository
{
    public class ChatContext : DbContext
    {
        public ChatContext(DbContextOptions<ChatContext> options) : base(options)
        {
        }

        public DbSet<ChatMessage> Messages
        {
            get; set;
        }
        public DbSet<User> Users
        {
            get; set;
        }
        public DbSet<Role> Roles
        {
            get; set;
        }
       
    }
}
