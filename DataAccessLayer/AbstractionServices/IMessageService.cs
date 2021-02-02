using DataAccessLayer.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.AbstractionServices
{
    public interface IMessageService
    {
        public Task Send(TextMessage textMessage);
    }
}
