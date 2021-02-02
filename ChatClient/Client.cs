using DataAccessLayer.Model;
using System;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading;

namespace ChatClient
{
    class Client
    {
        static string userName;
        private const string host = "127.0.0.1";
        private const int port = 1313;
        static TcpClient client;
        static NetworkStream stream;

        static void Main(string[] args)
        {
            Console.Write("Введите свое имя: ");
            userName = Console.ReadLine();
            Console.Write("Введите свой пароль: ");
            var password = Console.ReadLine();

            client = new TcpClient();
            try
            {
                client.Connect(host, port);
                stream = client.GetStream();
                SendRegMessage(userName, password);
                Thread receiveThread = new Thread(new ThreadStart(ReceiveMessage));
                receiveThread.Start();
                Console.WriteLine("Добро пожаловать, {0}", userName);
                SendMessage();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Disconnect();
            }
        }
        static void SendMessage()
        {
            Console.WriteLine("Введите сообщение: ");

            while (true)
            {
                string Message = MakeMessage();
                byte[] data = Encoding.UTF8.GetBytes(Message);
                stream.Write(data, 0, data.Length);
            }
        }

        private static string MakeMessage()
        {
            Console.Write(" Выберите номер группу: ");
            var groupId = Console.ReadLine();
            Console.Write("Введите сообщение: ");
            var text = Console.ReadLine();
            var grp = groupId.Equals("0") ? (long?)null : long.Parse(groupId);
            var message = new TextMessage { GroupId  = grp,
                                            UserName = userName, 
                                            Body = text, 
                                            CreateDate = DateTime.Now  };
            string json = JsonSerializer.Serialize(message);

            return json;
        }

        static void ReceiveMessage()
        {
            while (true)
            {
                try
                {
                    byte[] data = new byte[512];
                    StringBuilder builder = new StringBuilder();
                    int bytes = 0;
                    do
                    {
                        bytes = stream.Read(data, 0, data.Length);
                        builder.Append(Encoding.UTF8.GetString(data, 0, bytes));
                    }
                    while (stream.DataAvailable);

                    string message = builder.ToString();
                    if (message.Equals("Хотите зарегистироваться y/n"))
                    {
                        var answer = Console.ReadLine();
                        if (answer.Equals("n"))
                        {
                            Disconnect();
                        }
                        else
                        {
                            Console.Write("Введите свой пароль еще раз: ");
                            var password = Console.ReadLine();
                            SendRegMessage(userName, password);
                           
                        }
                    }
                    Console.WriteLine(message);
                }
                catch
                {
                    Console.WriteLine("Подключение прервано!");
                    Console.ReadLine();
                    Disconnect();
                }
            }
        }

        private static void SendRegMessage(string userName, string password)
        {
            var regOrAuthMessage = ClientHelper.GetRegOrAuthMessage(userName, password);
            string json = JsonSerializer.Serialize(regOrAuthMessage);
            byte[] authData = Encoding.UTF8.GetBytes(json);
            stream.Write(authData, 0, authData.Length);
        }

        static void Disconnect()
        {
            if (stream != null)
                stream.Close();
            if (client != null)
                client.Close();
            Environment.Exit(0);
        }
    }
}
