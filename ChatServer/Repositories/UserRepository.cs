using ChatServer.DTO;
using DataAccessLayer;
using DataAccessLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatServer.Repositories
{
    public class UserRepository
    {
        private ChatContext _chatContext { get; set; } = new ChatContext();
        public async Task<User> CreateUser(User user)
        {
            try
            {
                var group = user.Group == null ? new Group { Id = 1, Name = "Common" } : user.Group;
                user.Group = group;
                await _chatContext.Users.AddAsync(user);
                await _chatContext.Groups.AddAsync(group);
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

        /**
         * PAssword
         * */
        public async Task<User> GetUserByNameAndPassword(Message<AuthMessage> authorizationMessage)
        {
            var result = _chatContext.Users.Where(w => w.Name == authorizationMessage.Loggin && w.Pass == authorizationMessage.Body.Pass).FirstOrDefault();

            return result;
        }

        public async Task UpdateUser(User user)
        {
            _chatContext.Users.Update(user);
            await _chatContext.SaveChangesAsync();
        }
    }
}
