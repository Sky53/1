using ChatServer.DataAccessLayer.Model;
using ChatServer.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatServer.DataAccessLayer.Repositories
{
    public class UserRepository
    {
        private ChatContext _chatContext { get; set; } = new ChatContext();
        public async Task<User> CreateUser(User user)
        {
            try
            {
                await _chatContext.Users.AddAsync(user);
                await _chatContext.SaveChangesAsync();
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
            }
            return user;
        }

        public async Task DeleteUser(User user)
        {
            _chatContext.Users.Remove(user);
            await _chatContext.SaveChangesAsync();
        }

        public async Task<UserDTO> GetUserByNameAndPassword(Message<AuthMessage> authorizationMessage)
        {
            try
            {
                var user = _chatContext.Users
                    .Include(i => i.Groups)
                    .Where(w => w.Name == authorizationMessage.Body.Login && w.Pass == authorizationMessage.Body.Pass)
                    .FirstOrDefault();
                List<BaseMessage> meaasge = null;
                if (user == null)
                {
                    throw new Exception("Not found");
                }
                var userGroupId = user.Groups.FirstOrDefault()?.Id;
                if (userGroupId != 0)
                {
                    if (userGroupId != authorizationMessage.GroupId)
                    {
                        var oldGroup = _chatContext.Groups.Find(userGroupId);

                        if (oldGroup != null)
                            user.Groups.Remove(oldGroup);

                        var newGroup = _chatContext.Groups.Find(authorizationMessage.GroupId);

                        if (newGroup != null)
                            user.Groups.Add(newGroup);

                        _chatContext.SaveChanges();
                    }
                }
                if (user != null)
                    meaasge = _chatContext.BaseMessages.Where(w => w.Type == 2 && w.UserId == user.Id).ToList();
                return new UserDTO
                {
                    Id = user.Id,
                    GroupId = user.Groups.FirstOrDefault()?.Id,
                    Name = user.Name,
                    Messages = meaasge.OrderByDescending(x => x.CreateDate).Skip(0).Take(10).Select(s => s.Body).ToList()
                };
            }
            catch (Exception wx)
            {
                Console.WriteLine();
                throw;
            }
        }

        public async Task UpdateUser(User user)
        {
            _chatContext.Users.Update(user);
            await _chatContext.SaveChangesAsync();
        }
    }
}
