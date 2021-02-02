using DataAccessLayer.AbstractionServices;
using DataAccessLayer.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Services
{
    public class MessageService : IMessageService
    {
        public Task Send(TextMessage textMessage)
        {
            throw new NotImplementedException();
        }
    }
}
