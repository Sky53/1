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
        private readonly UserService _userService;
        private readonly Mock<IUserRepository> _userRepositoryMock = new Mock<IUserRepository>();

        public UserServiceTest()
        {
            _userService = new UserService(_userRepositoryMock.Object);
        }

        [Fact]
        public async void PositiveCaseAuthorizationUser()
        {
            // Arrange
            var userName = "Test";

            var user = new User
            {
                Id = 1,
                Name = userName,
                Groups = new List<Group> {new Group {Id = 1}}
            };

            var authMessage = new Message<AuthMessage>
            {
                Type = (int) MessageType.Authorization,
                Body = new AuthMessage
                {
                    Login = userName,
                    Pass = "pass"
                },
                GroupId = 1
            };

            _userRepositoryMock.Setup(m => m.GetUserByNameAndPassword(It.IsAny<Message<AuthMessage>>()))
                .ReturnsAsync(user);

            _userRepositoryMock.Setup(m => m.ChangeUserGroup(It.IsAny<long>(), It.IsAny<long?>()))
                .ReturnsAsync(new Group
                {
                    Id = 1,
                    Users = new List<User> { new User { Id = 1, Name = "Test" } }
                });

            // Act
            var result = await _userService.Auth(authMessage);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(result.Name, authMessage.Body.Login);
            Assert.Equal(result.GroupId, authMessage.GroupId);
        }

        [Fact]
        public async void NegativeCaseAuthorizationUser()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await _userService.Auth(null));
        }
    }
}