using ChatServer.DataAccessLayer.Model;
using ChatServer.DataAccessLayer.Repositories;
using ChatServer.DTO;
using ChatServer.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ChatServerTest
{
    public class UserServiceTest
    {
        private UserService _userService;

        public UserServiceTest()
        {
            _userService = new UserService(new UserRepository());
        }

        [Fact]
        public async void PositiveCaseAuthorizationUser()
        {
            var authMessage = new Message<AuthMessage>
            {
                Type = (int) MessageType.Authorization,
                Body = new AuthMessage
                {
                    Login = "User1",
                    Pass = "pass"
                },
                GroupId = 1
            };

            var mock = new Mock<IUserRepository>();
            mock.Setup(m => m.GetUserByNameAndPassword(It.IsAny<Message<AuthMessage>>()))
                .Returns(Task.FromResult(new User
                {
                    Id = 1,
                    Name = "Test",
                    Groups = new List<Group> { new Group { Id = 1 } }
                }));

            mock.Setup(m => m.ChangeUserGroup(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(Task.FromResult(new Group
                {
                    Id = 1,
                    Users = new List<User> { new User { Id = 1, Name = "Test" } }
                }));

            var user = mock.Object.GetUserByNameAndPassword(authMessage);
            var group = mock.Object.ChangeUserGroup(0,0);
            _userService = new UserService(mock.Object);
            var result = await _userService.Auth(authMessage);
            Assert.True(result != null);
        }

        [Fact]
        public async void NegativeCaseAuthorizationUser()
        {
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(async () => await _userService.Auth(null));

            Assert.Equal("Value cannot be null. (Parameter 'message')", ex.Message);
        }
    }
}