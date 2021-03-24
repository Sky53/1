using ChatServer.DataAccessLayer.Model;
using ChatServer.DataAccessLayer.Repositories;
using ChatServer.DTO;
using ChatServer.Exceptions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChatServer.Services
{
    public class UserService
    {
        private readonly UserRepository _userRepository;
        private const int MessagesCount = 10;

        public UserService(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserDto> Auth(Message<AuthMessage> message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            var user = await _userRepository.GetUserByNameAndPassword(message);

            if (user == null)
            {
                throw new UserNotFoundException("User with this login and password combination wasn't found");
            }

            var newGroup = await _userRepository.ChangeUserGroup(user.Id, message.GroupId);
            if (newGroup == null)
            {
                throw new GroupNotFoundException($"Group with id = {message.GroupId} not founded");
            }

            var oldMessages = await _userRepository.GetLastMessages(user.Id, MessagesCount);

            return new UserDto
            {
                Id = user.Id,
                GroupId = newGroup.Id,
                Messages = oldMessages
            };
        }
    }
}