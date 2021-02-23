using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DataAccessLayer.Model
{
    public class BaseMessage
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string Loggin { get; set; }
        public DateTime CreateDate { get; set; }
        public int Type { get; set; }
        public string Body { get; set; }

        public long? GroupId { get; set; }
        public Group? Group { get; set; }
        public List<User> Users { get; set; } = new List<User>();
    }
}
