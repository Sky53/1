using System;
using System.Threading;
using ChatServer.DataAccessLayer.Repositories;
using ChatServer.Services;

namespace ChatServer
{
    public class Program
    {
        private static readonly MessageRepository MessageRepository = new MessageRepository();
        private static readonly UserRepository UserRepository = new UserRepository();
        private static readonly MessageService MessageService = new MessageService(MessageRepository);
        private static readonly UserService UserService = new UserService(UserRepository);
        
        static void Main(string[] args)
        {
            var server = new Server(MessageService, UserService);
            try
            {
                var listenThread = new Thread(server.ListenNewConnections);
                var receiveThread = new Thread(server.ReceivingMessagesFromClients);
                receiveThread.IsBackground = true;
                listenThread.Start();
                receiveThread.Start();
            }
            catch (Exception ex)
            {
                server.Disconnect();
                Console.WriteLine(ex.Message);
            }
        }
    }
}
