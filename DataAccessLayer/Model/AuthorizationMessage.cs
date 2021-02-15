using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Model
{
    public class AuthorizationMessage : BaseMessage, IAuthorizationMessage
    {
        public string Pass { get; set; }
        public bool IsReg { get; set; }

        Guid BaseMessageId { get; set; }
        BaseMessage BaseMessage { get; set; }

        public AuthorizationMessage() { }
        public AuthorizationMessage(string name, string pass, bool isReg = false, int groupId = 0) : base(name)
        {
            Pass = pass;
            IsReg = isReg;
            GroupId = groupId;
        }
    }
}
