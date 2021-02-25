using System;
using System.Collections.Generic;
using System.Text;

namespace ChatServer.DataAccessLayer.Model
{
    public enum MessageType
    {
        Authorization = 1,
        Text = 2,
        UserData = 3,
    }
}
