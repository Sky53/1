using ChatServer.DataAccessLayer.Model;
using System;
using System.Threading.Tasks;

namespace ChatServer.DataAccessLayer.Repositories
{
    public class MessageRepository
    {
        private readonly ChatContext _chatContext  = new ChatContext();
        public async Task CreateMessage(BaseMessage message)
        {
            await _chatContext.BaseMessages.AddAsync(message);
            await _chatContext.SaveChangesAsync();
        }
    }
}
