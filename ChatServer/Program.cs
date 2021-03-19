using System;
using System.Threading;

namespace ChatServer
{
    class Program
    {

        static void Main(string[] args)
        {
            var server = new Server();
            try
            {
                var listenThread = new Thread(server.ListenNewConnections);
                var receiveThread = new Thread(server.ReceivingMessagesFromUsers);
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
