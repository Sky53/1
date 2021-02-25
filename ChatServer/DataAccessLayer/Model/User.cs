using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ChatServer.DataAccessLayer.Model
{
    public class User
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string Name { get; set; }
        public string Pass { get; set; }

        public Guid? BaseMessageId { get; set; }
        public virtual ICollection<Group> Groups { get; set; } = new List<Group>();
    }
}
