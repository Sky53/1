using ChatServer.DataAccessLayer.Model;
using ChatServer.DTO;
using ChatServer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ChatServer
{
    public class ServerService
    {
        static TcpListener tcpListener;
        List<ClientService> clients = new List<ClientService>();
        private readonly MessageService messageService = new MessageService();
        private readonly UserService userService = new UserService();
        protected internal void AddConnection(ClientService clientObject)
        {
            clients.Add(clientObject);
        }
        protected internal void RemoveConnection(string sessionId)
        {
            ClientService client = clients.FirstOrDefault(c => c.SessionId == sessionId);
            if (client != null)
                clients.Remove(client);
        }
        protected internal void Listen()
        {
            try
            {
                tcpListener = new TcpListener(IPAddress.Any, 1313);
                tcpListener.Start();
                Console.WriteLine("Сервер запущен. Ожидание подключений...");
                while (true)
                {
                    TcpClient tcpClient = tcpListener.AcceptTcpClient();

                    ClientService clientObject = new ClientService(tcpClient, this);
                    Thread clientThread = new Thread(new ThreadStart(clientObject.Process));
                    clientThread.Start();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Disconnect();
            }
        }

        internal async Task SendUserData(Message<UserDTO> msq, string sessionId)
        {
            var user = JsonSerializer.Serialize(msq);
            byte[] data = Encoding.UTF8.GetBytes(user);
            var usr = clients.Where(w => w.SessionId == sessionId).FirstOrDefault();
            usr.groupId = (long)msq.GroupId;
            usr.Stream.Write(data, 0, data.Length);
        }

        internal async Task processingMessage(UserDTO user, Message<TxtMessage> objMsg)
        {
            objMsg.UserId = user.Id;
            BaseMessage message = ParseMessage(objMsg);
            await messageService.Send(message);
        }

        private BaseMessage ParseMessage(Message<TxtMessage> objMsg)
        {
            return new BaseMessage
            {
                UserId = objMsg.UserId,
                CreateDate = DateTime.Now,
                Type = 2,
                Body = objMsg.Body.Text,
                GroupId = objMsg.GroupId ?? null
            };
        }

        internal async Task<UserDTO> AuthorizationUser(Message<AuthMessage> msg)
        {
            return await userService.Auth(msg); 
        }

        protected internal void BroadcastMessage(string message, string id, long? groupq = 0)
        {
            var users = groupq == null ? clients : clients.Where(w => w.groupId == groupq).ToList();
            byte[] data = Encoding.UTF8.GetBytes(message);
            for (int i = 0; i < users.Count; i++)
            {
                if (users[i].SessionId != id)
                {
                    users[i].Stream.Write(data, 0, data.Length);
                }
            }
        }

        internal void SendError(string sessionId)
        {
            var response = "Exit";
            byte[] data = Encoding.UTF8.GetBytes(response);
            var user = clients.Where(w => w.SessionId == sessionId).FirstOrDefault();
            user.Stream.Write(data, 0, data.Length);
        }
        protected internal void Disconnect()
        {
            tcpListener.Stop();

            for (int i = 0; i < clients.Count; i++)
            {
                clients[i].Close();
            }
            Environment.Exit(0);
        }
    }
}
