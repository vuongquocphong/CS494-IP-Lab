using System.Net;
using System.Reflection.Metadata;
using System.Text;
using Messages;
using Sockets;

namespace GameServer
{
    public class ServerEntryPoint
    {
        static int m_Count = 0;
        private readonly MessageFactory MessageFactory = new();
        private static ServerHandler ServerHandler = new();
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Instantiate a CSocketServer object
            SocketServer socketServer = new();
            // Start listening for connections
            socketServer.Start(IPAddress.Parse("127.0.0.1"), 9000, 1024,
                new MessageHandler(MessageHandlerServer),
                new SocketServer.AcceptHandler(AcceptHandler),
                new CloseHandler(CloseHandler),
                new ErrorHandler(ErrorHandler));
            Console.WriteLine("Waiting for a client connection on Machine: {0} Port: {1}", Environment.MachineName, 9000);
            // Stay here until you are ready to shutdown the server    
            Console.ReadLine();
            socketServer.Dispose();
        }

        public static void ErrorHandler(SocketBase socket, Exception pException)
        {
            Console.WriteLine(pException.Message);
        }
        public static void CloseHandler(SocketBase socket)
        {
            Console.WriteLine("Close Handler");
            Console.WriteLine("IpAddress: " + socket.IpAddress);
        }

        public static void PrintByteArray(byte[] bytes)
        {
            var sb = new StringBuilder("new byte[] { ");
            foreach (var b in bytes)
            {
                sb.Append(b + ", ");
            }
            sb.Append('}');
            Console.WriteLine(sb.ToString());
        }

        /// <summary> Called when a message is extracted from the socket </summary>
        /// <param name="pSocket"> The SocketClient object the message came from </param>
        /// <param name="iNumberOfBytes"> The number of bytes in the RawBuffer inside the SocketClient </param>
        public static void MessageHandlerServer(SocketBase socket, int iNumberOfBytes)
        {
            try
            {
                SocketClient pSocket = (SocketClient)socket;
                // Find a complete message
                // PrintByteArray(pSocket.RawBuffer);
                byte[] message = pSocket.RawBuffer[0..iNumberOfBytes];

                PrintByteArray(message);

                Message msg = MessageFactory.CreateMessage(message);

                Console.WriteLine("Message=<{1}> Received {0} messages", ++m_Count, msg.MessageType);

                // Send a response
                switch (msg.MessageType)
                {
                    case MessageType.ClientConnectionMessage:
                        HandleClientConnection(pSocket, (ClientConnectionMessage)msg);
                        break;
                    case MessageType.Guess:
                        GuessMessage clientGuessMessage = (GuessMessage)msg;
                        Console.WriteLine("ClientGuessMessage: {0}", clientGuessMessage.Guess);
                        break;
                    case MessageType.Ready:
                        ReadyMessage clientReadyMessage = (ReadyMessage)msg;
                        Console.WriteLine("ClientReadyMessage: {0}", clientReadyMessage.Ready);
                        break;
                    case MessageType.Timeout:
                        TimeoutMessage timeoutMessage = (TimeoutMessage)msg;
                        Console.WriteLine("TimeoutMessage: {0}", timeoutMessage);
                        break;
                    default:
                        Console.WriteLine("Unknown message type");
                        break;
                }
            }
            catch (Exception pException)
            {
                Console.WriteLine(pException.Message);
            }
        }

        /// <summary> Called when a socket connection is accepted </summary>
        /// <param name="pSocket"> The SocketClient object the message came from </param>
        static public void AcceptHandler(SocketClient pSocket)
        {
            
            Console.WriteLine("Accept Handler");
            Console.WriteLine("IpAddress: " + pSocket.IpAddress + ":" + pSocket.Port);
        }

        public static void HandleClientConnection(SocketClient pSocket, ClientConnectionMessage message)
        {
            AddPlayerResult result = ServerHandler.AddPlayer(message.Username);
            switch (result)
            {
                case AddPlayerResult.ServerIsFull:
                    pSocket.Send(
                        new ServerConnectionFailureMessage(
                            ErrorCode.ServerIsFull, "Server is full"
                        ).Serialize()
                    );
                    break;
                case AddPlayerResult.NameAlreadyTaken:
                    pSocket.Send(
                        new ServerConnectionFailureMessage(
                            ErrorCode.NameAlreadyTaken, "Name already taken"
                        ).Serialize()
                    );
                    break;
                case AddPlayerResult.GameInProgress:
                    pSocket.Send(
                        new ServerConnectionFailureMessage(
                            ErrorCode.GameInProgress, "Game is already in progress"
                        ).Serialize()
                    );
                    break;
                case AddPlayerResult.Success:
                    pSocket.Send(new ServerConnectionSuccessMessage().Serialize());
                    break;
            }
        }

        public static void HandleGuess(SocketClient pSocket, GuessMessage message)
        {
            GuessResult result = ServerHandler.Guess(message.PlayerName, message.Type, message.Guess);
            switch (result)
            {
                case GuessResult.Invalid:
                    pSocket.Send(
                        new GuessResponseMessage(
                            message.PlayerName, message.Type, message.Guess, GuessResult.Invalid
                        ).Serialize()
                    );
                    break;
                case GuessResult.Correct:
                    pSocket.Send(
                        new GuessResponseMessage(
                            message.PlayerName, message.Type, message.Guess, GuessResult.Correct
                        ).Serialize()
                    );
                    break;
                case GuessResult.Incorrect:
                    pSocket.Send(
                        new GuessResponseMessage(
                            message.PlayerName, message.Type, message.Guess, GuessResult.Incorrect
                        ).Serialize()
                    );
                    break;
            }
        }
    }
}