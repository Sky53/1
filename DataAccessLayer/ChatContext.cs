using DataAccessLayer.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;


namespace DataAccessLayer
{
    public class ChatContext : DbContext
    {


        public DbSet<User> Users { get; set; }
        public DbSet<BaseMessage> BaseMessages { get; set; }
        public DbSet<Group> Groups { get; set; }

        public ChatContext()
        {
            Database.Migrate();
            //Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql("Host = localhost; Port = 5432; Database = Chat; Username = postgres; Password = fgtkmcby");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Group>().HasData(
                new Group { Name = "group1", Id = 1 },
                new Group { Name = "group2", Id = 2 },
                new Group { Name = "group3", Id = 3 },
                new Group { Name = "group4", Id = 4 },
                new Group { Name = "group5", Id = 5 }
                );
            modelBuilder.Entity<User>().HasData(
                new User { Name = "User1", Pass = "pass", GroupId = 1, Id = 1 },
                new User { Name = "User2", Pass = "pass", GroupId = 2, Id = 2 },
                new User { Name = "User3", Pass = "pass", GroupId = 3, Id = 3 },
                new User { Name = "User4", Pass = "pass", GroupId = 4, Id = 4 },
                new User { Name = "User5", Pass = "pass", GroupId = 5, Id = 5 }
                );
            //modelBuilder.Entity<AuthorizationMessage>().ToTable("AuthorizationMessage");
            //modelBuilder.Entity<TextMessage>().ToTable("TextMessage");
        }
    }
}
