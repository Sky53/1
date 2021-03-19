using ChatServer.DTO;
using System;
using System.Net.Sockets;

namespace ChatServer
{
    public class Client
    {
        // TODO: SessionId public, Stream private
        protected internal string SessionId { get; private set; }
        protected internal NetworkStream Stream { get; private set; }
        public string UserName;
        public UserDto UserDto;
        public long? GroupId ;
        private readonly TcpClient _tcpClient;

        public Client(TcpClient tcpClient)
        {
            SessionId = Guid.NewGuid().ToString();
            _tcpClient = tcpClient;
            Stream = _tcpClient.GetStream();
        }

        protected internal void Close()
        {
            Stream?.Close();
            _tcpClient?.Close();
        }
    }
}
