using DataAccessLayer.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Abstraction
{
    public interface IMessageRepository
    {
        public Task<List<TextMessage>> GetMessagesForGroup(long id);
        public Task<Guid> CreateMessage(TextMessage message);
        public Task UpdateMessage(TextMessage message);
        public Task DeleteMessage(TextMessage message);
    }
}
