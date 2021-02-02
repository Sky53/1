using DataAccessLayer.Abstraction;
using DataAccessLayer.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private ChatContext _chatContext { get; set; } = new ChatContext();
        public async Task<Guid> CreateMessage(TextMessage message)
        {
            await _chatContext.TextMessages.AddAsync(message);
            await _chatContext.SaveChangesAsync();

            return message.Id;
        }

        public async Task DeleteMessage(TextMessage user)
        {
            _chatContext.TextMessages.Remove(user);
            await _chatContext.SaveChangesAsync();
        }

        public async Task<List<TextMessage>> GetMessagesForGroup(long id)
        {
            var result =  await _chatContext.TextMessages.ToListAsync();
            await _chatContext.SaveChangesAsync();

            return id == 0 ? result : result.Where(w => w.GroupId == id).ToList();
        }

        public async Task UpdateMessage(TextMessage message)
        {
            _chatContext.TextMessages.Update(message);
            await _chatContext.SaveChangesAsync();
        }
    }
}
