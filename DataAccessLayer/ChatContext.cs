using DataAccessLayer.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;


namespace DataAccessLayer
{
    public class ChatContext :  DbContext
    {


        public DbSet<User> Users { get; set; }
        public DbSet<BaseMessage> BaseMessages { get; set; }
        public DbSet<AuthorizationMessage> AuthorizationMessages { get; set; }
        public DbSet<TextMessage> TextMessages { get; set; }
        public DbSet<Group> Groups { get; set; }

        public ChatContext()
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql("Host = localhost; Port = 5432; Database = Chat; Username = postgres; Password = fgtkmcby");
            }
        }
    }
}
