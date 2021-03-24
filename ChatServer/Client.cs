using ChatServer.DTO;
using System;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace ChatServer
{
    public class Client
    {
        public string SessionId { get; private set; }
        private NetworkStream _stream { get; }
        public UserDto UserDto;
        private readonly TcpClient _tcpClient;

        public Client(TcpClient tcpClient)
        {
            SessionId = Guid.NewGuid().ToString();
            _tcpClient = tcpClient;
            _stream = _tcpClient.GetStream();
        }

        public bool AvailableMessage()
        {
            return _stream.DataAvailable;
        }

        public int ReadMessageBytesCount(byte[] messageInBytes)
        {
            return _stream.Read(messageInBytes, 0, messageInBytes.Length);
        }
        
        public async Task SendMessageAsync(byte[] messageBytes)
        {
            await _stream.WriteAsync(messageBytes, 0, messageBytes.Length);
        }
        
        protected internal void Close()
        {
            _stream?.Close();
            _tcpClient?.Close();
        }
    }
}