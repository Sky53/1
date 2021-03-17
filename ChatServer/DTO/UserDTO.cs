using ChatServer.DataAccessLayer.Model;
using System.Collections.Generic;

namespace ChatServer.DTO
{
    public class UserDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Pass { get; set; }
        public long? GroupId { get; set; }
        public List<string> Messages { get; set; }
    }
}