﻿using DataAccessLayer.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Abstraction
{
    public interface IUserRepository
    {
        public Task<User> GetUserByNameAndPassword(AuthorizationMessage authorizationMessage);
        public Task<long> CreateUser(User user);
        public Task UpdateUser(User user);
        public Task DeleteUser(User user);
    }
}