using ChatServer.DTO;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using ChatServer.DataAccessLayer.Model;
using ChatServer.Exceptions;

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

                var userGroupId = user.Groups.FirstOrDefault()?.Id;
                if (userGroupId != null && userGroupId != authorizationMessage.GroupId)
                {
                    var oldGroup = await _chatContext.Groups.FindAsync(userGroupId);

                    if (oldGroup != null)
                        user.Groups.Remove(oldGroup);

                    var newGroup = await _chatContext.Groups.FindAsync(authorizationMessage.GroupId);

                    if (newGroup != null)
                        user.Groups.Add(newGroup);

                    await _chatContext.SaveChangesAsync();
                }

                var userMessages = await _chatContext.BaseMessages
                    .Where(w => w.Type == (int)MessageType.Text && w.UserId == user.Id)
                    .OrderByDescending(m => m.CreateDate)
                    .Take(MessagesCount)
                    .Select(m => m.Body)
                    .ToListAsync();

                return new UserDto
                {
                    Id = user.Id,
                    GroupId = user.Groups.FirstOrDefault()?.Id,
                    Name = user.Name,
                    Messages = userMessages
                };
            }
            catch
            {
                throw;
            }
        }
    }
}