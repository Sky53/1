using ChatClient.Model;

namespace ChatClient
{
    public static class ClientHelper
    {
        public static Message<AuthMessage> GetAuthenticationMessage(string userName, string password, int groupId = 0)
        {
            return new Message<AuthMessage>
            {
                Body = new AuthMessage
                {
                    Login = userName,
                    Pass = password
                },
                GroupId = groupId,
                Type = (int) MessageType.Authorization
            };
        }
    }
}