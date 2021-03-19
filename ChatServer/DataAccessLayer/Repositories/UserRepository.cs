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
        private const int MessagesCount = 10;

        private readonly ChatContext _chatContext = new ChatContext();

        public async Task<UserDto> GetUserByNameAndPassword(Message<AuthMessage> authorizationMessage)
        {
            try
            {
                var user = await _chatContext.Users
                    .Include(i => i.Groups)
                    .FirstOrDefaultAsync(w =>
                        w.Name == authorizationMessage.Body.Login && w.Pass == authorizationMessage.Body.Pass);

                if (user == null)
                {
                    throw new UserNotFoundException("User with this login and password combination wasn't found");
                }

                return new UserDto
                {
                    Id = user.Id,
                    GroupId = user.Groups.FirstOrDefault()?.Id,
                    Name = user.Name
                };
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<string>> GetLastMessages(long userId)
        {
            var oldNessage = await _chatContext.BaseMessages
                    .Where(w => w.Type == (int)MessageType.Text && w.UserId == userId)
                    .OrderByDescending(m => m.CreateDate)
                    .Take(MessagesCount)
                    .Select(m => m.Body)
                    .ToListAsync();

            return oldNessage.ToList();
        }

        public async Task<Group> ChangeUserGroup(UserDto userDto, long targetGroupId)
        {
            var user = await _chatContext.Users.FindAsync(userDto.Id);
            var oldGroup = await _chatContext.Groups.FindAsync(userDto.GroupId);

            if (oldGroup != null)
            {
                user.Groups.Remove(oldGroup);
            }

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