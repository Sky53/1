using DataAccessLayer.AbstractionServices;
using DataAccessLayer.Model;
using DataAccessLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Services
{
    public class UserService : IUserService
    {
        private readonly UserRepository userRepository = new UserRepository();
        public async Task<User> Auth(AuthorizationMessage message)
        {
            if (message == null)
                throw new ArgumentNullException();
            var result = await userRepository.GetUserByNameAndPassword(message);

            return result;
        }

        public async Task<User> Create(User user)
        {
            if (user == null)
                throw new ArgumentException("Value can not be empty");
            var newUserID = await userRepository.CreateUser(user);

            return newUserID;
        }
    }
}
