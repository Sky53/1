using System;

namespace ChatServer.DTO
{
    public class Message<T>
    {
        public string Login { get; set; }
        public long UserId { get; set; }
        public DateTime CreateDate { get; set; }
        public int Type { get; set; }
        public T Body { get; set; }
        public long? GroupId { get; set; }
    }
}
