using DataAccessLayer.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.AbstractionServices
{
    public interface IUserService
    {
        public bool Auth(string name, string pass);
        public Task<User> Create(User user);
    }
}
