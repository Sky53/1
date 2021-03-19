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
        public string UserName;
        public UserDto UserDto;
        public long? GroupId;
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

        protected internal void Close()
        {
            _stream?.Close();
            _tcpClient?.Close();
        }

        public int GetBytesCount(byte[] messageInBytes)
        {
            return _stream.Read(messageInBytes, 0, messageInBytes.Length);
        }

        public async Task SendUserData(byte[] userDataDtoBytes)
        {
            await _stream.WriteAsync(userDataDtoBytes, 0, userDataDtoBytes.Length);
        }

        public async Task BroadcastMessageAsync(byte[] messageBytes)
        {
            await _stream.WriteAsync(messageBytes, 0, messageBytes.Length);
        }

        public async Task SendError(byte[] rejectedMessageBytes)
        {
            await _stream.WriteAsync(rejectedMessageBytes, 0, rejectedMessageBytes.Length);
        }
    }
}