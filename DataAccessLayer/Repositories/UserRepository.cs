using DataAccessLayer.Abstraction;
using DataAccessLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class UserRepository : IUserRepository
    {
        private ChatContext _chatContext { get; set; } = new ChatContext();
        public async Task<long> CreateUser(User user)
        {
            await _chatContext.Users.AddAsync(user);
            await _chatContext.SaveChangesAsync();

            return user.Id;
        }

        public async Task DeleteUser(User user)
        {
            _chatContext.Users.Remove(user);
            await _chatContext.SaveChangesAsync();
        }

        public async Task<User> GetUserByNameAndPassword(AuthorizationMessage authorizationMessage)
        {
            var result = _chatContext.Users.Where(w => w.Name == authorizationMessage.UserName && w.Pass == authorizationMessage.Pass).FirstOrDefault();
            
            return result;
        }

        public async Task UpdateUser(User user)
        {
            _chatContext.Users.Update(user);
            await _chatContext.SaveChangesAsync();
        }
    }
}
