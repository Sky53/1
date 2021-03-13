using ChatServer.DataAccessLayer;
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
                var listenThread = new Thread(server.Listen);
                var reciveThred = new Thread(server.ReceivingMessage);
                reciveThred.IsBackground = true;
                listenThread.Start();
                reciveThred.Start();
            }
            catch (Exception ex)
            {
                server.Disconnect();
                Console.WriteLine(ex.Message);
            }
        }
    }
}
