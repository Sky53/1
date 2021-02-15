using DataAccessLayer.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.AbstractionServices
{
    public interface IUserService
    {
        public Task<User> Auth(AuthorizationMessage message);
        public Task<User> Create(User user);
    }
}
