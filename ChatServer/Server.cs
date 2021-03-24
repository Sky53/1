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
        private readonly MessageService _messageService;
        private readonly UserService _userService;

        public Server(MessageService messageService, UserService userService)
        {
            _messageService = messageService;
            _userService = userService;
        }

        public void ListenNewConnections()
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

        public async void ReceivingMessagesFromClients()
        {
            while (true)
            {
                if (_clients.Count == 0)
                {
                    continue;
                }

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
                            await AnalyseAndProcessFirstMessage(messageFromClient, client);
                        }
                        else
                        {
                            var receivedMessage = MessageTextParse(messageFromClient);
                            await ProcessingMessage(client.UserDto, receivedMessage);

                            var messageToOtherClients = $"{client.UserDto.Name}: {receivedMessage.Body.Text}";
                            Console.WriteLine(messageToOtherClients);
                            await BroadcastMessageAsync(messageToOtherClients, client.SessionId,
                                receivedMessage.GroupId);
                        }
                    }
                    catch (Exception ex) when (ex is UserNotFoundException || ex is GroupNotFoundException || ex is ArgumentNullException)
                    {
                        SendError(client.SessionId);
                        _clients.Remove(client);
                        client.Close();
                    }
                    catch (Exception)
                    {
                        var msg = $"{client.UserDto?.Name}: покинул чат";
                        Console.WriteLine(msg);
                        await BroadcastMessageAsync(msg, client.SessionId);
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
            if (!client.AvailableMessage())
            {
                return null;
            }

            var messageInBytes = new byte[512];
            var builder = new StringBuilder();

            do
            {
                var bytesCount = client.ReadMessageBytesCount(messageInBytes);
                builder.Append(Encoding.UTF8.GetString(messageInBytes, 0, bytesCount));
            } while (client.AvailableMessage());

            return builder.ToString();
        }

        private async Task AnalyseAndProcessFirstMessage(string msg, Client client)
        {
            var authMessage = JsonSerializer.Deserialize<Message<AuthMessage>>(msg);
            var user = await _userService.Auth(authMessage);

            client.UserDto = user;

            var message = client.UserDto.Name + " вошел в чат";

            await SendUserDataMessage(user, client);
            await BroadcastMessageAsync(message, client.SessionId);
            Console.WriteLine(message);
        }

        private static async Task SendUserDataMessage(UserDto user, Client client)
        {
            var message = new Message<UserDto>
            {
                Login = user.Name,
                Type = (int) MessageType.UserData,
                Body = user,
                GroupId = user.GroupId
            };

            var userDataMessage = JsonSerializer.Serialize(message);
            var userDataMessageInBytes = Encoding.UTF8.GetBytes(userDataMessage);

            await client.SendMessageAsync(userDataMessageInBytes);
        }

        private async Task ProcessingMessage(UserDto user, Message<TxtMessage> receivedMessage)
        {
            receivedMessage.UserId = user.Id;
            await _messageService.Save(receivedMessage);
        }

        private async Task BroadcastMessageAsync(string message, string sessionId, long? groupId = null)
        {
            var clients = groupId == null ? _clients : _clients.Where(w => w.UserDto.GroupId == groupId).ToList();
            var messageBytes = Encoding.UTF8.GetBytes(message);

            foreach (var client in clients.Where(client => client.SessionId != sessionId))
            {
                await client.SendMessageAsync(messageBytes);
            }
        }

        private async void SendError(string sessionId)
        {
            var rejectedMessageBytes = Encoding.UTF8.GetBytes("Exit");
            var client = _clients.FirstOrDefault(w => w.SessionId == sessionId);
            if (client != null)
            {
                await client?.SendMessageAsync(rejectedMessageBytes);
            }
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