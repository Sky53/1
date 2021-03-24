using ChatServer.DataAccessLayer.Model;
using ChatServer.DataAccessLayer.Repositories;
using System;
using System.Threading.Tasks;

namespace ChatServer.Services
{
    public class MessageService
    {
        private readonly MessageRepository _messageRepository;

        public MessageService(MessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }
        public async Task Send(BaseMessage textMessage)
        {
            if (textMessage == null)
            {
                throw new ArgumentNullException(nameof(textMessage));
            }

            await _messageRepository.CreateMessage(textMessage);
        }
    }
}
