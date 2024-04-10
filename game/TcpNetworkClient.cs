using System.Net;
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

        public void MessageHandler(SocketBase socket, int iNumberOfBytes)
        {
            try
            {
                SocketClient pSocket = (SocketClient)socket;
                byte[] message = pSocket.RawBuffer[0..iNumberOfBytes];

                // Message msg = MessageFactory.CreateMessage(message);

                Receive(message);
            }
            catch (Exception pException)
            {
                Console.WriteLine(pException.Message);
            }
        }

        public static void CloseHandler(SocketBase pSocket)
        {
            Console.WriteLine("Close Handler");
            Console.WriteLine("IpAddress: " + pSocket.IpAddress);
        }

        public static void ErrorHandler(SocketBase pSocket, Exception pException)
        {
            Console.WriteLine(pException.Message);
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

        public void Receive(byte[] message)
        {
            Mediator.Notify(this, MessageFactory.CreateMessage(message));
            GD.Print("Received message");
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
