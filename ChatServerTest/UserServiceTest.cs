using ChatServer.DataAccessLayer.Model;
using ChatServer.DataAccessLayer.Repositories;
using ChatServer.DTO;
using ChatServer.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ChatServerTest
{
    public class UserServiceTest
    {
        private static readonly UserRepository UserRepository = new UserRepository();
        private readonly UserService _userService = new UserService(UserRepository);

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
            var result = true;

            try
            {
                await _userService.Auth(null);
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
