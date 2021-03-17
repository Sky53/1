using System;

namespace ChatServer.Exceptions
{
    public class UserNotFoundException : Exception
    {
        public UserNotFoundException() : base()
        {
        }

        public UserNotFoundException(string message) : base(message)
        {
        }
    }
}