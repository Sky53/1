using ChatServer.DataAccessLayer;
using System;
using System.Threading;

namespace ChatServer
{
    class Program
    {
        static ServerService server; // сервер
        static Thread listenThread; // потока для прослушивания
        static void Main(string[] args)
        {
            try
            {
                new ChatContext();
                server = new ServerService();
                listenThread = new Thread(new ThreadStart(server.Listen));
                listenThread.Start(); //старт потока
            }
            catch (Exception ex)
            {
                server.Disconnect();
                Console.WriteLine(ex.Message);
            }
        }
    }
}
