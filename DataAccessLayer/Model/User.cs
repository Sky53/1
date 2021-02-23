using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DataAccessLayer.Model
{
    public class User
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string Name { get; set; }
        public string Pass { get; set; }

        public long GroupId { get; set; }
        public Group Group { get; set; }
        public Guid? BaseMessageId { get; set; }
        public List<BaseMessage> BaseMessage { get; set; } = new List<BaseMessage>();
    }
}
