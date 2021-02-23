﻿using ChatServer.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatClient
{
    public class ClientHelper
    {
        internal static Message<AuthMessage> GetRegOrAuthMessage(string userName, string password, int groupID = 0)
        {
            return new Message<AuthMessage> { Body = new AuthMessage { Pass = password, Login = userName  }, GroupId = groupID, Type = 1 };
        }
    }
}
