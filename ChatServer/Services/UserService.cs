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
        private readonly UserRepository _userRepository = new UserRepository();
        public async Task<UserDto> Auth(Message<AuthMessage> message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            return await _userRepository.GetUserByNameAndPassword(message);
        }

        public async Task ChangeUserGroup(long targetGroupId, UserDto userDto)
        {
            var newGroup = await _userRepository.ChangeUserGroup(userDto, targetGroupId);

            if (newGroup != null)
            {
                userDto.GroupId = newGroup.Id;
            }
            else 
            {
                throw new GroupNotFoundException($"Group with id = {targetGroupId} not founded");
            }
        }

        public async Task GetLastMessages(UserDto userDto)
        {
            var oldMessages = await _userRepository.GetLastMessages(userDto.Id);
            userDto.Messages = oldMessages;
        }
    }
}
