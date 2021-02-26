using ChatServer.DataAccessLayer.Model;
using ChatServer.DataAccessLayer.Repositories;
using ChatServer.DTO;
using System;
using System.Threading.Tasks;

namespace ChatServer.Services
{
    public class UserService
    {
        private readonly UserRepository userRepository = new UserRepository();
        public async Task<UserDTO> Auth(Message<AuthMessage> message)
        {
            if (message == null)
                throw new ArgumentNullException();
            try
            {
                var result = await userRepository.GetUserByNameAndPassword(message);

                return result;
            }
            catch { }
            return null;
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
