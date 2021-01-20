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

        public void Process()
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

                            var statusAuth = server.AuthorizationUser(obj, this.SessionId);
                            if (statusAuth)
                            {
                                userName = obj.UserName;//userdata
                                msg = userName + " вошел в чат";
                                server.BroadcastMessage(msg, this.SessionId);
                                Console.WriteLine(msg);
                            }
                            else
                            {
                                server.SendOffer(msg, this.SessionId);
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
                    catch
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
            return builder.ToString();
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
