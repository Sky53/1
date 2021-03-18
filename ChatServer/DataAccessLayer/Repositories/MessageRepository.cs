using ChatServer.DataAccessLayer.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatServer.DataAccessLayer.Repositories
{
    public class MessageRepository
    {
        private readonly ChatContext _chatContext  = new ChatContext();
        public async Task<Guid> CreateMessage(BaseMessage message)
        {
            await _chatContext.BaseMessages.AddAsync(message);
            await _chatContext.SaveChangesAsync();

            return message.Id;
        }
    }
}
