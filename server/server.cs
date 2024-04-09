using System.Net;
using System.Text;
using Messages;
using Sockets;

namespace GameServer
{
    public class ServerEntryPoint
    {
        static int m_Count = 0;
        private static readonly ServerHandler ServerHandler = new();
        static readonly SocketServer socketServer = new();
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Instantiate a CSocketServer object
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
            Console.WriteLine("IpAddress: " + socket.IpAddress + ':' + socket.Port);
            socketServer.RemoveSocket((SocketClient)socket);
            ServerHandler.Disconnect(socket.IpAddress.ToString() + ':' + socket.Port);
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
                    case MessageType.Ready:
                        HandleReady(pSocket, (ReadyMessage)msg);
                        break;
                    case MessageType.Guess:
                        HandleGuess(pSocket, (GuessMessage)msg);
                        break;
                    case MessageType.Timeout:
                        HandleTimeout(pSocket, (TimeoutMessage)msg);
                        break;
                    case MessageType.ClientDisconnect:
                        CloseHandler(pSocket);
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
            string address = pSocket.IpAddress.ToString() + ":" + pSocket.Port;
            AddPlayerResult result = ServerHandler.AddPlayer(address, message.Username);
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
                    socketServer.NotifyConnectedClients(
                        new PlayerListMessage(ServerHandler.GetPlayerReadyList()).Serialize()
                    );
                    break;
            }
        }

        public static void HandleReady(SocketClient pSocket, ReadyMessage message)
        {
            string address = pSocket.IpAddress.ToString() + ":" + pSocket.Port;
            ServerHandler.Ready(address, message.Ready);
            socketServer.NotifyConnectedClients(
                new PlayerListMessage(ServerHandler.GetPlayerReadyList()).Serialize()
            );
            int readyCount = 0;
            for (int i = 0; i < ServerHandler.Players.Count; i++)
            {
                if (ServerHandler.Players[i].State == ServerPlayerState.Ready)
                {
                    readyCount++;
                }
            }
            if (readyCount == ServerHandler.Players.Count)
            {
                ServerHandler.StartGame();
                socketServer.NotifyConnectedClients(
                    new GameStartedMessage(ServerHandler.KeyWord, ServerHandler.Hint).Serialize()
                );
            }
        }

        public static void HandleGuess(SocketClient pSocket, GuessMessage message)
        {
            string address = pSocket.IpAddress.ToString() + ":" + pSocket.Port;
            string playerName = ServerHandler.GetPlayer(address)!.Username;
            GuessResult result = ServerHandler.Guess(address, message.GuessType, message.Guess);
            switch (result)
            {
                case GuessResult.Invalid:
                    socketServer.NotifyConnectedClients(
                        new GuessResultMessage(GuessResult.Invalid, message.GuessType, playerName, message.Guess
                        ).Serialize()
                    );
                    break;
                case GuessResult.Correct:
                    socketServer.NotifyConnectedClients(
                        new GuessResultMessage(GuessResult.Correct, message.GuessType, playerName, message.Guess
                        ).Serialize()
                    );
                    break;
                case GuessResult.Incorrect:
                    socketServer.NotifyConnectedClients(
                        new GuessResultMessage(GuessResult.Incorrect, message.GuessType, playerName, message.Guess
                        ).Serialize()
                    );
                    break;
                case GuessResult.Duplicate:
                    socketServer.NotifyConnectedClients(
                        new GuessResultMessage(GuessResult.Duplicate, message.GuessType, playerName, message.Guess
                        ).Serialize()
                    );
                    break;
            }
            if (ServerHandler.ServerState == ServerState.GameOver)
            {
                socketServer.NotifyConnectedClients(
                    new GameResultMessage(ServerHandler.GetResults()).Serialize()
                );
            }
            else
            {
                socketServer.NotifyConnectedClients(
                    new GameStatusMessage(
                        ServerHandler.Players.Count,
                        (int)ServerHandler.CurrentGameTurn,
                        (int)ServerHandler.CurrentPlayerTurn,
                        ServerHandler.KeyWord,
                        ServerHandler.GetPlayerInfoList()
                    ).Serialize()
                );
                ServerHandler.NextTurn();
            }
        }

        public static void HandleTimeout(SocketClient pSocket, TimeoutMessage _)
        {
            string address = pSocket.IpAddress.ToString() + ":" + pSocket.Port;
            if (ServerHandler.Players[(int)ServerHandler.CurrentPlayerTurn].PlayerId != address)
            {
                return;
            }
            ServerHandler.NextTurn();
            if (ServerHandler.ServerState == ServerState.GameOver)
            {
                socketServer.NotifyConnectedClients(
                    new GameResultMessage(ServerHandler.GetResults()).Serialize()
                );
            }
            else
            {
                socketServer.NotifyConnectedClients(
                    new GameStatusMessage(
                        ServerHandler.Players.Count,
                        (int)ServerHandler.CurrentGameTurn,
                        (int)ServerHandler.CurrentPlayerTurn,
                        ServerHandler.KeyWord,
                        ServerHandler.GetPlayerInfoList()
                    ).Serialize()
                );
                ServerHandler.NextTurn();
            }
        }
    }
}