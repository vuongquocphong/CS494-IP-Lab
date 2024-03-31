using System.Net;
using System.Net.Sockets;

namespace Sockets
{
    public class SocketServer : SocketBase
    {
        private const int maxConnections = 10;
        private SocketClient[] connections = new SocketClient[maxConnections];
        private Socket listener;

        // private Thread acceptThread;

        public SocketServer(IPAddress ipAddress, int port, MessageHandler messageHandler, CloseHandler closeHandler, ErrorHandler errorHandler) : base(ipAddress, port, messageHandler, closeHandler, errorHandler)
        {
            listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            listener.Bind(new IPEndPoint(ipAddress, port));
        }

        public void StartListening()
        {
            listener.Listen(maxConnections);

            while (true)
            {
                Socket handler = listener.Accept();
                for (int i = 0; i < maxConnections; i++)
                {
                    if (connections[i] == null && handler.RemoteEndPoint != null)
                    {
                        IPAddress ipAddress = ((IPEndPoint)handler.RemoteEndPoint).Address;
                        int port = ((IPEndPoint)handler.RemoteEndPoint).Port;
                        connections[i] = new SocketClient(ipAddress, port, messageHandler, closeHandler, errorHandler);
                        break;
                    }
                }
            }
        }

        public static void Send(SocketClient connection, byte[] data)
        {
            connection.Send(data);
        }

        public static void Receive(SocketClient connection)
        {
            connection.Receive();
        }

        public override void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
