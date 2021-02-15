using DataAccessLayer.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatClient
{
    public class ClientHelper
    {
        internal static AuthorizationMessage GetRegOrAuthMessage(string userName, string password, bool isReg = false, int groupID = 0)
        {
            return new AuthorizationMessage (userName, password, isReg = isReg, groupID);
        }
    }
}
