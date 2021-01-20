using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Model
{
    public class Group
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string Name { get; set; }


        //public long UserID { get; set; }
        //public User User { get; set; }
        public Guid MessagBaseMessageID { get; set; }
        public BaseMessage BaseMessage { get; set; }
    }
}