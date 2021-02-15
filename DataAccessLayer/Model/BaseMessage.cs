using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DataAccessLayer.Model
{
    public abstract class BaseMessage
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string SessionId { get; set; }
        public DateTime CreateDate { get; set; }
        public MessageType Type { get; set; }

        public long? GroupId { get; set; }
        public Group? Group { get; set; }
        public List<User> Users { get; set; } = new List<User>();

        public BaseMessage()
        {
        }
        public BaseMessage(string name) 
        {
            UserName = name;
        }
    }
}
