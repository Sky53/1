using ChatServer.DataAccessLayer.Model;
using ChatServer.DataAccessLayer.Repositories;
using ChatServer.DTO;
using ChatServer.Services;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ChatServerTest
{
    public class MessageServiceTest
    {
        private readonly MessageService _messageService;
        private readonly Mock<IMessageRepository> _messageMockRepository = new Mock<IMessageRepository>();

        public MessageServiceTest()
        {
            _messageService = new MessageService(_messageMockRepository.Object);
        }

        [Fact]
        public async void PositiveCaseSaveMessage()
        {
            // Arrange
            var textMessage = new Message<TxtMessage>
            {
                Type = (int)MessageType.Text,
                Body = new TxtMessage
                {
                    Text = "Hi, i'm test message"
                },
                UserId = 1,
                GroupId = null
            };

            _messageMockRepository.Setup(m => m.CreateMessage(It.IsAny<BaseMessage>()))
                .Returns(Task.CompletedTask);

            // Act
            await _messageService.Save(textMessage);

            // Assert
            _messageMockRepository.Verify(m => m.CreateMessage(It.IsAny<BaseMessage>()), Times.Once);
        }

        [Fact]
        public async void NegativeCaseSaveMessage()
        {
            _messageMockRepository.Setup(m => m.CreateMessage(It.IsAny<BaseMessage>()))
                .Returns(Task.CompletedTask);

            await Assert.ThrowsAsync<ArgumentNullException>(async () => await _messageService.Save(null));
        }
    }
}
