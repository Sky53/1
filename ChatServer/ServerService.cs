using DataAccessLayer;
using DataAccessLayer.Model;
using DataAccessLayer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ChatServer
{
    public class ServerService
    {
        static TcpListener tcpListener; 
        List<ClientService> clients = new List<ClientService>();
        private readonly GroupService groupService = new GroupService();
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

        internal void SendResponsOnAuth(string userJson, string sessionId)
        {
            byte[] data = Encoding.UTF8.GetBytes(userJson);
            var user = clients.Where(w => w.SessionId == sessionId).FirstOrDefault();
            user.Stream.Write(data, 0, data.Length);
        }

        internal async Task<User> CreateUser(AuthorizationMessage user)
        {
            var res = await userService.Create(new User { Name = user.UserName,
                                                    Pass = user.Pass });

            return res;

        }

        internal bool AuthorizationUser(AuthorizationMessage msg, string sessionId)
        {
            var st = DALHelper.Authorization(msg);
            var result = userService.Auth(msg);
            return st == null ? false : true;
        }

        protected internal void BroadcastMessage(string message, string id, Group groupq = null)
        {
            var users = groupq == null ? clients : clients.Where(w => w.groupId == groupq.Id).ToList();
            byte[] data = Encoding.UTF8.GetBytes(message);
            for (int i = 0; i < users.Count; i++)
            {
                if (clients[i].SessionId != id)
                {
                    clients[i].Stream.Write(data, 0, data.Length);
                }
            }
        }

        internal void SendOffer(string sessionId)
        {
            var response = "Хотите зарегистироваться y/n";
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
