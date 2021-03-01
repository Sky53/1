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
                var ReciveThred = new Thread(server.Listen);
                listenThread.Start();
            }
            catch (Exception ex)
            {
                server.Disconnect();
                Console.WriteLine(ex.Message);
            }
        }
    }
}
