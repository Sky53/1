using ChatServer.DataAccessLayer.Model;
using ChatServer.DataAccessLayer.Repositories;
using System;
using System.Threading.Tasks;
using ChatServer.DTO;

namespace ChatServer.Services
{
    public class MessageService
    {
        private  IMessageRepository _messageRepository;

        public MessageService(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }

        public async Task Save(Message<TxtMessage> textMessage)
        {
            if (textMessage == null)
            {
                throw new ArgumentNullException(nameof(textMessage));
            }

            var baseMessage = new BaseMessage
            {
                UserId = textMessage.UserId,
                CreateDate = DateTime.Now,
                Type = (int) MessageType.Text,
                Body = textMessage.Body.Text,
                GroupId = textMessage.GroupId
            };

            await _messageRepository.CreateMessage(baseMessage);
        }
    }
}
