using ChatServer.DataAccessLayer.Model;
using ChatServer.DataAccessLayer.Repositories;
using System;
using System.Threading.Tasks;

namespace ChatServer.Services
{
    public class MessageService
    {
        public readonly MessageRepository messageRepository = new MessageRepository();
        public async Task Send(BaseMessage textMessage)
        {
            if (textMessage == null)
                throw new ArgumentException("Value can not be empty");

            textMessage.GroupId = textMessage.GroupId == 0 ? null : textMessage.GroupId;
            await messageRepository.CreateMessage(textMessage);
        }
    }
}
