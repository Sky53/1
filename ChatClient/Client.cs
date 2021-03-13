using ChatServer.DTO;
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
        public static UserDTO User = null;
        private const string host = "127.0.0.1";
        private const int port = 1313;
        static TcpClient client;
        static NetworkStream Stream;

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
                Stream = client.GetStream();
                SendRegMessage(userName, password, group: int.Parse(group));
                Thread receiveThread = new Thread(new ThreadStart(ReceiveMessage));
                receiveThread.Start();
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
                Stream.Write(data, 0, data.Length);
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
                UserId = User.Id
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
                        bytes = Stream.Read(data, 0, data.Length);
                        builder.Append(Encoding.UTF8.GetString(data, 0, bytes));
                    }
                    while (Stream.DataAvailable);

                    string message = builder.ToString();
                    if (message != null)
                    {
                        AnalysisMessage(message);
                    }
                   

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
                Console.WriteLine($"Welcome {User.Name}");
                if (user.Body.Messages != null )
                {
                    foreach (var item in user.Body.Messages)
                    {
                        Console.WriteLine(item);
                    }
                }
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
            Stream.Write(authData, 0, authData.Length);
        }

        static void Disconnect()
        {
            if (Stream != null)
                Stream.Close();
            if (client != null)
                client.Close();
            Environment.Exit(0);
        }
    }
}
