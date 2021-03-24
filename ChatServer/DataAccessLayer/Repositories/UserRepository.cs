using ChatServer.DTO;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using ChatServer.DataAccessLayer.Model;
using ChatServer.Exceptions;
using System.Collections.Generic;

namespace ChatServer.DataAccessLayer.Repositories
{
    public class UserRepository
    {
        private readonly ChatContext _chatContext = new ChatContext();

        public Task<User> GetUserByNameAndPassword(Message<AuthMessage> authorizationMessage)
        {
            return _chatContext.Users
                    .Include(i => i.Groups)
                    .FirstOrDefaultAsync(w =>
                        w.Name == authorizationMessage.Body.Login && w.Pass == authorizationMessage.Body.Pass);
        }

        public Task<List<string>> GetLastMessages(long userId, int messagesCount)
        {
            return  _chatContext.BaseMessages
                    .Where(w => w.Type == (int)MessageType.Text && w.UserId == userId)
                    .OrderByDescending(m => m.CreateDate)
                    .Take(messagesCount)
                    .Select(m => m.Body)
                    .ToListAsync();
        }

        public async Task<Group> ChangeUserGroup(long userId, long? targetGroupId)
        {
            var user = await _chatContext.Users.FindAsync(userId);

            if (user == null)
            {
                throw new UserNotFoundException($"User with ID={userId} doesn't exist");
            }

            user.Groups.Clear();

            var newGroup = await _chatContext.Groups.FindAsync(targetGroupId);
            if (newGroup != null)
            {
                user.Groups.Add(newGroup);
            }

            await _chatContext.SaveChangesAsync();

            return newGroup;
        }
    }
}