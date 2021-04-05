using ChatServer.DataAccessLayer.Model;
using ChatServer.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ChatServer.DataAccessLayer.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetUserByNameAndPassword(Message<AuthMessage> authorizationMessage);
        Task<List<string>> GetLastMessages(long userId, int messagesCount);
        Task<Group> ChangeUserGroup(long userId, long? targetGroupId);
    }
}
