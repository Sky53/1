using DataAccessLayer.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatClient
{
    public class ClientHelper
    {
        internal static AuthorizationMessage GetRegOrAuthMessage(string userName, string password)
        {
            return new AuthorizationMessage { UserName = userName, Pass = password };
        }
    }
}
