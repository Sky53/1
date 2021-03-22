using ChatServer.DTO;
using System;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using ChatServer.DataAccessLayer.Model;

namespace ChatClient
{
    class Client
    {
        private static string _userName;
        public static UserDto User = null;
        private const string Host = "127.0.0.1";
        private const int Port = 1313;
        private static TcpClient _currentClient;
        private static NetworkStream _stream;

        static void Main(string[] args)
        {
            Console.Write("Введите свое имя: ");
            _userName = Console.ReadLine();
            Console.Write("Введите свой пароль: ");
            var password = Console.ReadLine();
            Console.Write("Выберите ID своей группы: ");
            var group = Console.ReadLine();
            _currentClient = new TcpClient();
            try
            {
                _currentClient.Connect(Host, Port);
                _stream = _currentClient.GetStream();
                SendRegMessage(_userName, password, group: int.Parse(group));
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
                var Message = MakeMessage();
                var data = Encoding.UTF8.GetBytes(Message);
                _stream.Write(data, 0, data.Length);
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
                Login = _userName,
                Type = (int)MessageType.Text,
                Body = new TxtMessage { Text = text },
                CreateDate = DateTime.Now,
                UserId = User.Id
            };
            var json = JsonSerializer.Serialize(message);

            return json;
        }

        static void ReceiveMessage()
        {
            while (true)
            {
                try
                {
                    var data = new byte[512];
                    var builder = new StringBuilder();
                    var bytes = 0;
                   
                    do
                    {
                        bytes = _stream.Read(data, 0, data.Length);
                        builder.Append(Encoding.UTF8.GetString(data, 0, bytes));
                    }
                    while (_stream.DataAvailable);

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
                var user = JsonSerializer.Deserialize<Message<UserDto>>(message);
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
            var regOrAuthMessage = ClientHelper.GetRegOrAuthMessage(userName, password, groupId: group);
            var json = JsonSerializer.Serialize(regOrAuthMessage);
            var authData = Encoding.UTF8.GetBytes(json);
            _stream.Write(authData, 0, authData.Length);
        }

        static void Disconnect()
        {
            _stream?.Close();
            _currentClient?.Close();
            Environment.Exit(0);
        }
    }
}
