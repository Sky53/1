using DataAccessLayer;
using DataAccessLayer.Model;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace ChatServer
{
    public class ClientService
    {

        protected internal string SessionId { get; private set; }
        protected internal NetworkStream Stream { get; private set; }
        string userName;
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
                while (true)
                {
                    try
                    {
                        var msg = GetMessage();
                        if (msg.Contains("pass", StringComparison.InvariantCultureIgnoreCase))
                        {
                            AuthorizationMessage obj = MessageAuthParse(msg);
                            if (obj.IsReg == true)
                            {
                               var user =  server.CreateUser(obj);
                               var userJson =  JsonSerializer.Serialize(user);
                               server.SendResponsOnAuth(userJson, this.SessionId);
                            }
                            var statusAuth = await server.AuthorizationUser(obj, this.SessionId);
                            if (statusAuth != null)
                            {
                                userName = obj.UserName;//userdata
                                 obj.SessionId = SessionId;//userdata
                                groupId = statusAuth.Group.Id;
                                msg = userName + " вошел в чат";
                                server.BroadcastMessage(msg, this.SessionId, statusAuth.Group);
                                Console.WriteLine(msg);
                            }
                            else
                            {
                                server.SendOffer(this.SessionId);
                            }
                        }
                        if (msg.Contains("body", StringComparison.InvariantCultureIgnoreCase))
                        {
                            TextMessage obj = MessageTextParse(msg);
                            ChatContext.TextMessages.Add(obj);
                            ChatContext.SaveChangesAsync();
                            msg = String.Format("{0} {1}", userName, obj.Body);
                            Console.WriteLine(msg);
                            server.BroadcastMessage(msg, this.SessionId, obj.Group);
                        }
                       
                    }
                    catch(Exception wxc)
                    {
                        var msg = String.Format("{0}: покинул чат", userName);
                        Console.WriteLine(msg);
                        server.BroadcastMessage(msg, this.SessionId);
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

        private TextMessage MessageTextParse(string msg)
        {
            return JsonSerializer.Deserialize<TextMessage>(msg);
        }

        private static AuthorizationMessage MessageAuthParse(string msg)
        {
            return JsonSerializer.Deserialize<AuthorizationMessage>(msg);
        }

        /*
         * Вернуть обект, Создать метод передачи обекта для анализа
         */
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
