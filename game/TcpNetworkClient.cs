using System.Net;
using System.Text;
using Godot;
using Mediator;
using Messages;
using Sockets;

namespace NetworkClient
{
    public class TcpNetworkClient : INetworkClient
    {
        public IMediator Mediator { get; set; } = null!;

        public SocketClient SocketClient { get; private set; } = null!;

        public TcpNetworkClient()
        {
            SocketClient = new SocketClient(
                    10240,
                    new MessageHandler(MessageHandler),
                    new CloseHandler(CloseHandler),
                    new ErrorHandler(ErrorHandler)
                );
        }

        ~TcpNetworkClient()
        {
            Close();
        }

        public void Connect()
        {
            SocketClient ??= new SocketClient(
                    10240,
                    new MessageHandler(MessageHandler),
                    new CloseHandler(CloseHandler),
                    new ErrorHandler(ErrorHandler)
                );
            SocketClient.Connect(IPAddress.Parse("127.0.0.1"), 9000);
        }

        public static void PrintByteArray(byte[] bytes)
        {
            var sb = new StringBuilder("new byte[] { ");
            foreach (var b in bytes)
            {
                sb.Append(b + ", ");
            }
            sb.Append('}');
            GD.Print(sb.ToString());
        }

        public void MessageHandler(SocketBase socket, int iNumberOfBytes)
        {
            try
            {
                SocketClient pSocket = (SocketClient)socket;
                byte[] message = pSocket.RawBuffer[0..iNumberOfBytes];

                GD.Print("Received message: " + message[0]);
                PrintByteArray(message);
                Message msg = MessageFactory.CreateMessage(message);

                GD.Print("Received message: " + msg);

                Receive(msg);
            }
            catch (Exception pException)
            {
                GD.Print(pException);
            }
        }

        public static void CloseHandler(SocketBase pSocket)
        {
            GD.Print("Close Handler");
            GD.Print("IpAddress: " + pSocket.IpAddress);
        }

        public static void ErrorHandler(SocketBase pSocket, Exception pException)
        {
            GD.Print(pException);
        }

        public void Send(byte[] message)
        {
            if (SocketClient == null || SocketClient.ClientSocket == null || SocketClient.ClientSocket.Connected == false)
            {
                Connect();
                GD.Print("Connected to server");
            }
            SocketClient.Send(message);
            GD.Print("Sent message");
        }

        public void Receive(Message message)
        {
            Mediator.Notify(this, message);
        }

        public void Close()
        {
            SocketClient.Send(new ClientDisconnectMessage().Serialize());
            SocketClient.Disconnect();
            SocketClient.Dispose();
            SocketClient = null;
        }
    }
}
