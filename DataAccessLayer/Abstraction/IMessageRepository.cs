using DataAccessLayer.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Abstraction
{
    public interface IMessageRepository
    {
        public Task GetMessageForGroup(string name, string pass);
        public Task<int> CreateMessage(TextMessage message);
        public Task UpdateMessage(TextMessage user);
        public Task DeleteMessage(TextMessage user);
    }
}
