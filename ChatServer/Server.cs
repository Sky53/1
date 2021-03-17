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
        private List<Client> _clients = new List<Client>();
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

        public async void ReceivingMessage()
        {
            while (true)
            {
                if (_clients.Count != 0)
                {
                    foreach (var client in _clients.ToList()) //Concurrent for the poor
                    {
                        try
                        {
                            var msg = GetMessage(client);

                            if (msg == null) continue;

                            if (client.UserDto == null)
                            {
                                var user = await AnalysFirstMessage(msg);
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
                                var objMsg = MessageTextParse(msg);
                                await ProcessingMessage(client.UserDto, objMsg);
                                msg = $"{client.UserName}: {objMsg.Body.Text}";
                                Console.WriteLine(msg);
                                BroadcastMessage(msg, client.SessionId, objMsg.GroupId);
                            }
                        }
                        catch (UserNotFoundException exc)
                        {
                            SendError(client.SessionId);
                            _clients.Remove(client);
                            client.Close();
                            continue;
                        }
                        catch (ArgumentNullException exc)
                        {
                            SendError(client.SessionId);
                            _clients.Remove(client);
                            client.Close();
                            continue;
                        }
                        catch (Exception exc)
                        {
                            var msg = $"{client.UserName}: покинул чат";
                            Console.WriteLine(msg);
                            BroadcastMessage(msg, client.SessionId);
                            _clients.Remove(client);
                            client.Close();
                            continue;
                        }
                    }
                }
            }
        }

        private Message<TxtMessage> MessageTextParse(string msg)
        {
            return JsonSerializer.Deserialize<Message<TxtMessage>>(msg);
        }

        private string GetMessage(Client client)
        {
            if (!client.Stream.DataAvailable)
                return null;
            var data = new byte[512];
            var builder = new StringBuilder();
            var bytes = 0;
            do
            {
                bytes = client.Stream.Read(data, 0, data.Length);
                builder.Append(Encoding.UTF8.GetString(data, 0, bytes));
            } while (client.Stream.DataAvailable);

            return builder.ToString();
        }

        private async Task<UserDto> AnalysFirstMessage(string msg)
        {
            var regMsg = JsonSerializer.Deserialize<Message<AuthMessage>>(msg);

            return await AuthorizationUser(regMsg);
            ;
        }

        private async Task SendUserData(Message<UserDto> msg, string sessionId)
        {
            var userDataDto = JsonSerializer.Serialize(msg);
            var userDataDtoBytes = Encoding.UTF8.GetBytes(userDataDto);
            var client = _clients.FirstOrDefault(w => w.SessionId == sessionId);
            client.GroupId = (long) msg.GroupId;
            await client.Stream.WriteAsync(userDataDtoBytes, 0, userDataDtoBytes.Length);
        }

        private async Task ProcessingMessage(UserDto user, Message<TxtMessage> objMsg)
        {
            objMsg.UserId = user.Id;
            BaseMessage message = ParseMessage(objMsg);
            await _messageService.Send(message);
        }

        private BaseMessage ParseMessage(Message<TxtMessage> objMsg)
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