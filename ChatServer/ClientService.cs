using ChatServer.DTO;
using DataAccessLayer;
using DataAccessLayer.Model;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ChatServer
{
    public class ClientService
    {

        protected internal string SessionId { get; private set; }
        protected internal NetworkStream Stream { get; private set; }
        string userName;
        UserDTO UserDTO;
        public long groupId = 0;
        TcpClient client;
        ServerService server;
        protected ChatContext ChatContext = new ChatContext();

        public ClientService(TcpClient tcpClient, ServerService serverObject)
        {
            SessionId = Guid.NewGuid().ToString();
            client = tcpClient;
            server = serverObject;
            serverObject.AddConnection(this);
        }

        public async void Process()
        {
            try
            {
                Stream = client.GetStream();
                var authMSG = GetMessage();
                var user = await AnalysFirstMessage(authMSG);
                if (user == null)
                {
                    server.SendError(SessionId);
                    throw new ArgumentException();
                }
                userName = user.Name;
                string message = userName + " вошел в чат";
                var userDTO = new Message<UserDTO>
                {
                    Loggin = user.Name,
                    Type = 3,
                    Body = new UserDTO { Id = user.Id, GroupId = user.GroupId, Name = user.Name, Pass = user.Pass },
                    GroupId = user.GroupId
                };
                await server.SendUserData(userDTO, SessionId);
                server.BroadcastMessage(message, SessionId);
                Console.WriteLine(message);
                while (true)
                {
                    try
                    {
                        var msg = GetMessage();
                        var objMsg = MessageTextParse(msg);
                        await server.processingMessage(user, objMsg);
                        msg = String.Format("{0}: {1}", userName, objMsg.Body.Text);
                        Console.WriteLine(msg);
                        server.BroadcastMessage(message, SessionId);
                    }
                    catch (Exception wxc)
                    {
                        var msg = String.Format("{0}: покинул чат", userName);
                        Console.WriteLine(msg);
                        server.BroadcastMessage(msg, SessionId);
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                server.RemoveConnection(this.SessionId);
                Close();
            }
        }

        private async Task<User> AnalysFirstMessage(string msg)
        {
            var regMSG = JsonSerializer.Deserialize<Message<AuthMessage>>(msg);
            var status = await server.AuthorizationUser(regMSG);
            return status;
        }

        private Message<TxtMessage> MessageTextParse(string msg)
        {
            return JsonSerializer.Deserialize<Message<TxtMessage>>(msg);
        }

        private string GetMessage()
        {
            byte[] data = new byte[512];
            StringBuilder builder = new StringBuilder();
            int bytes = 0;
            do
            {
                bytes = Stream.Read(data, 0, data.Length);
                builder.Append(Encoding.UTF8.GetString(data, 0, bytes));
            }
            while (Stream.DataAvailable);

            var message = builder.ToString();
            return message;
        }
        protected internal void Close()
        {
            if (Stream != null)
                Stream.Close();
            if (client != null)
                client.Close();
        }
    }
}
