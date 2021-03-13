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
        public string userName;
        public UserDTO UserDTO;
        public long GroupId = 0;
        TcpClient client;

        public Client(TcpClient tcpClient, Server serverObject)
        {
            SessionId = Guid.NewGuid().ToString();
            client = tcpClient;
            serverObject.AddConnection(this);
            Stream = client.GetStream();
        }

        protected internal void Close()
        {
            if (Stream != null)
                Stream.Close();
            if (client != null)
                client.Close();
        }
    }
}
