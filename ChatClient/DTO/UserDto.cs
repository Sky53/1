using System.Collections.Generic;

namespace ChatClient.Model
{
    public class UserDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long? GroupId { get; set; }
        public List<string> Messages { get; set; }
    }
}