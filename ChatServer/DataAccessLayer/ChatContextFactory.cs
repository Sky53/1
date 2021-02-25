using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatServer.DataAccessLayer
{
    public class ChatContextFactory : IDesignTimeDbContextFactory<ChatContext>
    {
        public ChatContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ChatContext>();
            optionsBuilder.UseNpgsql("Host = localhost; Port = 5432; Database = Chat; Username = postgres; Password = fgtkmcby");

            return new ChatContext();
        }
    }
}
