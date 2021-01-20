using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Model
{
    public class TextMessage : BaseMessage , ITextMessage
    {
        public string Body { get; set; }

        Guid BaseMessageID { get; set; }
        BaseMessage BaseMessage { get; set; }
    }
}
