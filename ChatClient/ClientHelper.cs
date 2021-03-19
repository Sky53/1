using ChatServer.DTO;

namespace ChatClient
{
    public class ClientHelper
    {
        public static Message<AuthMessage> GetRegOrAuthMessage(string userName, string password, int groupId = 0)
        {
            return new Message<AuthMessage>
                     { Body = new AuthMessage { Pass = password, Login = userName }, GroupId = groupId, Type = 1 };
        }
    }
}
