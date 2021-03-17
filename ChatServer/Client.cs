using ChatServer.DataAccessLayer;
using ChatServer.DataAccessLayer.Model;
using ChatServer.DTO;
using System;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ChatServer
{
    public class Client
    {
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
