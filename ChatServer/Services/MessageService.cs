using ChatServer.DataAccessLayer.Model;
using ChatServer.DataAccessLayer.Repositories;
using System;
using System.Threading.Tasks;

namespace ChatServer.Services
{
    public class MessageService
    {
        private readonly MessageRepository _messageRepository = new MessageRepository();
        public async Task Send(BaseMessage textMessage)
        {
            if (textMessage == null)
                throw new ArgumentException("Value can not be empty");
            await _messageRepository.CreateMessage(textMessage);
        }
    }
}
