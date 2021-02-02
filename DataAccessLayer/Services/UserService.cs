using DataAccessLayer.AbstractionServices;
using DataAccessLayer.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Services
{
    public class UserService : IUserService
    {
        public bool Auth(string name, string pass)
        {
            throw new NotImplementedException();
        }

        public Task<User> Create(User user)
        {
            throw new NotImplementedException();
        }
    }
}
