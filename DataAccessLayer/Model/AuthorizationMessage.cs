using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Model
{
    public class AuthorizationMessage : BaseMessage, IAuthorizationMessage
    {
        public string Pass { get; set; }

        Guid BaseMessageId { get; set; }
        BaseMessage BaseMessage { get; set; }
    }
}
