using ChatServer.DataAccessLayer.Model;
using ChatServer.DataAccessLayer.Repositories;
using ChatServer.DTO;
using ChatServer.Services;
using System;
using Xunit;

namespace ChatServerTest
{
    public class MessageServiceTest
    {
        private static readonly MessageRepository MessageRepository = new MessageRepository();
        private readonly MessageService _messageService = new MessageService(MessageRepository);

        [Fact]
        public async void PositiveCaseSaveMessage()
        {
            var result = true;
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

            try
            {
                await _messageService.Save(textMessage);
            }
            catch (Exception e)
            {
                result = false;
            }
            finally 
            {
                Assert.True(result);
            }
        }

        [Fact]
        public async void NegativeCaseSaveMessage()
        {
            var result = true;

            try
            {
                await _messageService.Save(null);
            }
            catch (Exception e)
            {
                result = false;
            }
            finally
            {
                Assert.False(result);
            }
        }
    }
}
