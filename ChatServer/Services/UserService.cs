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
                throw new ArgumentNullException("Value can not be empty");
          
            return await userRepository.GetUserByNameAndPassword(message);
        }
    }
}
