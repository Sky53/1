using DataAccessLayer.AbstractionServices;
using DataAccessLayer.Model;
using DataAccessLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Services
{
    public class MessageService : IMessageService
    {
        public readonly MessageRepository messageRepository = new MessageRepository();
        public async Task Send(TextMessage textMessage)
        {
            if (textMessage == null)
                throw new ArgumentException("Value can not be empty");

             await messageRepository.CreateMessage(textMessage);
        }
    }
}
