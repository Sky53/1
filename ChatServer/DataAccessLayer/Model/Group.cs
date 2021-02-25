using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ChatServer.DataAccessLayer.Model
{
    public class Group
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string Name { get; set; }

        public List<User> User { get; set; }
        //public Guid? MessagBaseMessageID { get; set; }
        //public BaseMessage BaseMessage { get; set; }
    }
}
