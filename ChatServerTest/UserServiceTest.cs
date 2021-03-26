using ChatServer.DataAccessLayer.Model;
using ChatServer.DataAccessLayer.Repositories;
using ChatServer.DTO;
using ChatServer.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ChatServerTest
{
    public class UserServiceTest
    {
        private readonly UserService _userService;

        public UserServiceTest()
        {
            _userService = new UserService(new UserRepository());
        }

        [Fact]
        public async void PositiveCaseAuthorizationUser()
        {
            var authMessage = new Message<AuthMessage>
            {
                Type = (int)MessageType.Authorization,
                Body = new AuthMessage
                {
                    Login = "User1",
                    Pass = "pass"
                },
                GroupId = 1
            };


            var user = await _userService.Auth(authMessage);

            Assert.True(user != null);

        }

        [Fact]
        public async void NegativeCaseAuthorizationUser()
        {
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(async () => await _userService.Auth(null));
            Assert.Equal("Value cannot be null. (Parameter 'message')", ex.Message);
        }
    }
}
