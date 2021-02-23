using ChatServer.Repositories;
using DataAccessLayer.Model;
using System;
using System.Collections.Generic;
using System.Text;
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

            await messageRepository.CreateMessage(textMessage);
        }
    }
}
