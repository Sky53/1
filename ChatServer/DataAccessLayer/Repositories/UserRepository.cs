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

        public async Task<UserDto> GetUserByNameAndPassword(Message<AuthMessage> authorizationMessage)
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

        public async Task<List<string>> GetLastMessages(long userId, int messagesCount)
        {
            return  await _chatContext.BaseMessages
                    .Where(w => w.Type == (int)MessageType.Text && w.UserId == userId)
                    .OrderByDescending(m => m.CreateDate)
                    .Take(messagesCount)
                    .Select(m => m.Body)
                    .ToListAsync();
        }

        public async Task<Group> ChangeUserGroup(UserDto userDto, long targetGroupId)
        {
            var user = await _chatContext.Users.FindAsync(userDto.Id);
            
            if (user == null)
            {
                throw new UserNotFoundException("User with this login and password combination wasn't found");
            }
            
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