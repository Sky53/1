using System;
using System.Collections.Generic;
using System.Text;

namespace ChatServer.DataAccessLayer.Model
{
    public class GroupUser
    {
        public long UserId { get; set; }
        public User User { get; set; }

        public long GroupId { get; set; }
        public Group Group { get; set; }
    }
}
