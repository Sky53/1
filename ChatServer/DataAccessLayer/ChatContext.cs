﻿using ChatServer.DataAccessLayer.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatServer.DataAccessLayer
{
    public class ChatContext : DbContext
    {


        public DbSet<User> Users { get; set; }
        public DbSet<BaseMessage> BaseMessages { get; set; }
        public DbSet<Group> Groups { get; set; }

        public ChatContext()
        {
            Database.Migrate();
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
             new User { Name = "User1", Pass = "pass", Id = 1 },
             new User { Name = "User2", Pass = "pass", Id = 2 },
             new User { Name = "User3", Pass = "pass", Id = 3 },
             new User { Name = "User4", Pass = "pass", Id = 4 },
             new User { Name = "User5", Pass = "pass", Id = 5 }
             );
            modelBuilder
                    .Entity<User>()
                    .HasMany(p => p.Groups)
                    .WithMany(p => p.Users)
                    .UsingEntity(j => j.HasData(
                        new  { UsersId = 1l, GroupsId = 1l },
                        new  { UsersId = 2l, GroupsId = 2l },
                        new  { UsersId = 3l, GroupsId = 3l },
                        new  { UsersId = 4l, GroupsId = 4l },
                        new  { UsersId = 5l, GroupsId = 5l }
                        ));
        }
    }
}