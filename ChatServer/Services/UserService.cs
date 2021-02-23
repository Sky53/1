using ChatServer.Repositories;
using DataAccessLayer.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ChatServer.Services
{
    public class UserService
    {
        private readonly UserRepository userRepository = new UserRepository();
        public async Task<User> Auth(BaseMessage message)
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
