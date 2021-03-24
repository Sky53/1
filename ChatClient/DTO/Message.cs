using System;

namespace ChatClient.Model
{
    public class Message<T>
    {
        public DateTime CreateDate { get; set; }
        public int Type { get; set; }
        public T Body { get; set; }
        public long? GroupId { get; set; }
    }
}