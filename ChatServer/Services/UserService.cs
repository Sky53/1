using ChatServer.DataAccessLayer.Model;
using ChatServer.DataAccessLayer.Repositories;
using ChatServer.DTO;
using System;
using System.Threading.Tasks;

namespace ChatServer.Services
{
    public class UserService
    {
        private readonly UserRepository _userRepository = new UserRepository();
        public async Task<UserDto> Auth(Message<AuthMessage> message)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));
          
            return await _userRepository.GetUserByNameAndPassword(message);
        }
    }
}
