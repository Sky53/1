using System;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using ChatClient.Model;

namespace ChatClient
{
    public class Client
    {
        private const string Host = "127.0.0.1";
        private const int Port = 1313;

        private static TcpClient _currentClient;
        private static NetworkStream _stream;

        private static UserDto _userDto;

        public static void Main(string[] args)
        {
            Console.Write("Введите свое имя: ");
            var userName = Console.ReadLine();

            Console.Write("Введите свой пароль: ");
            var password = Console.ReadLine();

            Console.Write("Выберите ID своей группы: ");
            var groupId = Console.ReadLine();

            _currentClient = new TcpClient();

            try
            {
                _currentClient.Connect(Host, Port);
                _stream = _currentClient.GetStream();

                SendRegMessage(userName, password, int.Parse(groupId));

                var receiveThread = new Thread(ReceiveMessage);
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

        private static void SendMessage()
        {
            while (true)
            {
                var message = MakeMessage();

                var data = Encoding.UTF8.GetBytes(message);
                _stream.Write(data, 0, data.Length);
            }
        }

        private static string MakeMessage()
        {
            Console.Write("Введите сообщение: ");
            var text = Console.ReadLine();

            Console.Write("для общей группы y/n: ");
            var forAllInString = Console.ReadLine();

            var forAll = forAllInString != null && forAllInString.Contains("y", StringComparison.InvariantCultureIgnoreCase);

            var message = new Message<TxtMessage>
            {
                GroupId = forAll ? null : _userDto.GroupId,
                Login = _userDto.Name,
                Type = (int) MessageType.Text,
                Body = new TxtMessage
                {
                    Text = text
                },
                CreateDate = DateTime.Now
            };

            return JsonSerializer.Serialize(message);
        }

        static void ReceiveMessage()
        {
            while (true)
            {
                if (!_stream.DataAvailable)
                {
                    continue;
                }

                var data = new byte[512];
                var builder = new StringBuilder();

                try
                {
                    do
                    {
                        var bytes = _stream.Read(data, 0, data.Length);
                        builder.Append(Encoding.UTF8.GetString(data, 0, bytes));
                    } while (_stream.DataAvailable);

                    var message = builder.ToString();
                    if (!string.IsNullOrEmpty(message))
                    {
                        ParseMessage(message);
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Подключение прервано!");
                    Console.ReadLine();
                    Disconnect();
                }
            }
        }

        private static void ParseMessage(string message)
        {
            if (message.Equals("Exit"))
            {
                Disconnect();
            }

            if (message.Contains("\"Type\":3"))
            {
                var userMessage = JsonSerializer.Deserialize<Message<UserDto>>(message);
                _userDto = userMessage.Body;

                Console.WriteLine($"Welcome {_userDto.Name}");

                if (_userDto.Messages != null)
                {
                    foreach (var item in _userDto.Messages)
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

        private static void SendRegMessage(string userName, string password, int groupId = 0)
        {
            var authMessage = ClientHelper.GetAuthenticationMessage(userName, password, groupId);
            var json = JsonSerializer.Serialize(authMessage);
            var authData = Encoding.UTF8.GetBytes(json);

            _stream.Write(authData, 0, authData.Length);
        }

        private static void Disconnect()
        {
            _stream?.Close();
            _currentClient?.Close();
            Environment.Exit(0);
        }
    }
}