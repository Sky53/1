using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ChatServer.DataAccessLayer.Model;
using ChatServer.DTO;
using ChatServer.Exceptions;
using ChatServer.Services;

namespace ChatServer
{
    public class Server
    {
        private static TcpListener _tcpListener;
        private readonly List<Client> _clients = new List<Client>();
        private readonly MessageService _messageService = new MessageService();
        private readonly UserService _userService = new UserService();

        public void Listen()
        {
            try
            {
                _tcpListener = new TcpListener(IPAddress.Any, 1313);
                _tcpListener.Start();
                Console.WriteLine("Сервер запущен. Ожидание подключений...");

                while (true)
                {
                    var tcpClient = _tcpListener.AcceptTcpClient();
                    var client = new Client(tcpClient);
                    _clients.Add(client);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Disconnect();
            }
        }

        public async void ReceivingMessages()
        {
            while (true)
            {
                if (_clients.Count == 0)
                {
                    continue;
                };

                foreach (var client in _clients.ToList()) //Concurrent for the poor
                {
                    try
                    {
                        var messageFromClient = GetMessage(client);

                        if (messageFromClient == null)
                        {
                            continue;
                        }

                        if (client.UserDto == null)
                        {
                            // TODO: To new method!!!
                            var user = await AnalyseFirstMessage(messageFromClient);
                            client.UserDto = user;
                            client.UserName = user.Name;
                            client.GroupId = (long) user.GroupId;
                            var message = client.UserName + " вошел в чат";
                            var userDto = new Message<UserDto>
                            {
                                Login = user.Name,
                                Type = 3,
                                Body = user,
                                GroupId = user.GroupId
                            };

                            await SendUserData(userDto, client.SessionId);
                            BroadcastMessage(message, client.SessionId);
                            Console.WriteLine(message);
                        }
                        else
                        {
                            var receivedMessage = MessageTextParse(messageFromClient);
                            await ProcessingMessage(client.UserDto, receivedMessage);

                            // TODO: ПОЛНЫЙ ППЦ!!! Почему используем эту переменную?
                            messageFromClient = $"{client.UserName}: {receivedMessage.Body.Text}";
                            Console.WriteLine(messageFromClient);
                            BroadcastMessage(messageFromClient, client.SessionId, receivedMessage.GroupId);
                        }

                    }
                    catch (UserNotFoundException)
                    {
                        SendError(client.SessionId);
                        _clients.Remove(client);
                        client.Close();
                    }
                    catch (ArgumentNullException)
                    {
                        SendError(client.SessionId);
                        _clients.Remove(client);
                        client.Close();
                    }
                    catch (Exception)
                    {
                        var msg = $"{client.UserName}: покинул чат";
                        Console.WriteLine(msg);
                        BroadcastMessage(msg, client.SessionId);
                        _clients.Remove(client);
                        client.Close();
                    }
                }
            }
        }

        private static Message<TxtMessage> MessageTextParse(string message)
        {
            return JsonSerializer.Deserialize<Message<TxtMessage>>(message);
        }

        private static string GetMessage(Client client)
        {
            if (!client.Stream.DataAvailable)
            {
                return null;
            }

            var messageInBytes = new byte[512];
            var builder = new StringBuilder();

            do
            {
                var bytesCount = client.Stream.Read(messageInBytes, 0, messageInBytes.Length);
                builder.Append(Encoding.UTF8.GetString(messageInBytes, 0, bytesCount));
            } while (client.Stream.DataAvailable);

            return builder.ToString();
        }

        private async Task<UserDto> AnalyseFirstMessage(string msg)
        {
            var regMsg = JsonSerializer.Deserialize<Message<AuthMessage>>(msg);

            return await AuthorizationUser(regMsg);
        }

        private async Task SendUserData(Message<UserDto> msg, string sessionId)
        {
            var userDataDto = JsonSerializer.Serialize(msg);
            var userDataDtoBytes = Encoding.UTF8.GetBytes(userDataDto);
            var client = _clients.FirstOrDefault(w => w.SessionId == sessionId);
            // TODO: на null проверить
            client.GroupId = (long) msg.GroupId;
            // TODO: инкапсулировать Stream полностью
            await client.Stream.WriteAsync(userDataDtoBytes, 0, userDataDtoBytes.Length);
        }

        private async Task ProcessingMessage(UserDto user, Message<TxtMessage> receivedMessage)
        {
            receivedMessage.UserId = user.Id;
            var message = ParseMessage(receivedMessage);
            await _messageService.Send(message);
        }

        private static BaseMessage ParseMessage(Message<TxtMessage> objMsg)
        {
            return new BaseMessage
            {
                UserId = objMsg.UserId,
                CreateDate = DateTime.Now,
                Type = 2,
                Body = objMsg.Body.Text,
                GroupId = objMsg.GroupId
            };
        }

        private async Task<UserDto> AuthorizationUser(Message<AuthMessage> msg)
        {
            return await _userService.Auth(msg);
        }

        private void BroadcastMessage(string message, string sessionId, long? groupId = null)
        {
            var clients = groupId == null ? _clients : _clients.Where(w => w.GroupId == groupId).ToList();
            var messageBytes = Encoding.UTF8.GetBytes(message);

            foreach (var client in clients.Where(client => client.SessionId != sessionId))
            {
                client.Stream.Write(messageBytes, 0, messageBytes.Length);
            }
        }

        private void SendError(string sessionId)
        {
            var rejectedMessageBytes = Encoding.UTF8.GetBytes("Exit");
            var user = _clients.FirstOrDefault(w => w.SessionId == sessionId);
            // TODO: инкапсулировать
            user?.Stream.Write(rejectedMessageBytes, 0, rejectedMessageBytes.Length);
        }

        protected internal void Disconnect()
        {
            _tcpListener.Stop();

            foreach (var client in _clients)
            {
                client.Close();
            }

            Environment.Exit(0);
        }
    }
}