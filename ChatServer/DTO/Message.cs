using System;
using System.Collections.Generic;
using System.Text;

namespace ChatServer.DTO
{
    public class Message<T>
    {
        public string Loggin { get; set; }
        public DateTime CreateDate { get; set; }
        public int Type { get; set; }
        public T Body { get; set; }
        public long? GroupId { get; set; }
    }
}
