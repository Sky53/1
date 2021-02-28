using ChatServer.DataAccessLayer;
using System;
using System.Threading;

namespace ChatServer
{
    class Program
    {
        static ServerService server;
        static Thread listenThread;
        static void Main(string[] args)
        {
            try
            {
                server = new ServerService();
                listenThread = new Thread(new ThreadStart(server.Listen));
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
