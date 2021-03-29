using ChatServer.DataAccessLayer.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ChatServer.DataAccessLayer.Repositories
{
    public interface IMessageRepository
    {
        public Task CreateMessage(BaseMessage message);
    }
}
