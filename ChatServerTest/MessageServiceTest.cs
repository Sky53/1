using ChatServer.DataAccessLayer.Model;
using ChatServer.DataAccessLayer.Repositories;
using ChatServer.DTO;
using ChatServer.Services;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ChatServerTest
{
    public class MessageServiceTest
    {
        private readonly MessageService _messageService;

        public MessageServiceTest()
        {

            _messageService = new MessageService(new MessageRepository());
        }

        [Fact]
        public async void PositiveCaseSaveMessage()
        {
            var textMessage = new Message<TxtMessage>
            {
                Type = (int)MessageType.Text,
                Body = new TxtMessage
                {
                    Text = "Hi, i'm test message"
                },
                UserId = 1,//Id for test
                GroupId = null
            };
            await Assert.IsAssignableFrom<Task>(_messageService.Save(textMessage));
        }

        [Fact]
        public async void NegativeCaseSaveMessage()
        {
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(async () => await _messageService.Save(null));
            Assert.Equal("Value cannot be null. (Parameter 'textMessage')", ex.Message);
            var result = true;
        }
    }
}
