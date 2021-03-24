using System;
using System.Collections.Generic;
using System.Text;

namespace ChatServer.Exceptions
{
    public class GroupNotFoundException : Exception
    {
        public GroupNotFoundException(string message) : base(message)
        {
        }
    }
}
