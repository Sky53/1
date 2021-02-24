using ChatServer.DTO;
using ChatServer.Services;
using DataAccessLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading;

namespace ChatClient
{
    class Client
    {
        static string userName;
        static bool IsActiv;
        public static UserDTO User = null;
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
            Console.Write("Выберите ID своей группы: ");
            var group = Console.ReadLine();
            client = new TcpClient();
            try
            {
                client.Connect(host, port);
                stream = client.GetStream();
                Thread receiveThread = new Thread(new ThreadStart(ReceiveMessage));
                receiveThread.Start();
                SendRegMessage(userName, password, group: int.Parse(group));
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
            while (true)
            {
                string Message = MakeMessage();
                byte[] data = Encoding.UTF8.GetBytes(Message);
                stream.Write(data, 0, data.Length);
            }
        }

        private static string MakeMessage()
        {
            Console.Write("Введите сообщение: ");
            var text = Console.ReadLine();
            Console.Write("для общей группы y/n: ");
            var group = Console.ReadLine();
            var forAll = group.Contains("y", StringComparison.InvariantCultureIgnoreCase);
            var message = new Message<TxtMessage>
            {
                GroupId = forAll ? null : User.GroupId,
                Loggin = userName,
                Type = 2,
                Body = new TxtMessage { Text = text },
                CreateDate = DateTime.Now,
            };
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
                    AnalysisMessage(message);

                }
                catch (Exception e)
                {
                    Console.WriteLine("Подключение прервано!");
                    Console.ReadLine();
                    Disconnect();
                }
            }
        }

        private static void AnalysisMessage(string message)
        {
            if (message.Equals("Exit"))
            {
                Disconnect();
            }
            if (message.Contains("\"Type\":3"))
            {
                var user = JsonSerializer.Deserialize<Message<UserDTO>>(message);
                User = user.Body;
                IsActiv = true;
                Console.WriteLine($"Welcome {User.Name}");
            }
            else
            {
                Console.WriteLine(message);
            }
        }

        private static void SendRegMessage(string userName, string password, bool isReg = false, int group = 0)
        {
            var regOrAuthMessage = ClientHelper.GetRegOrAuthMessage(userName, password, groupID: group);
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
