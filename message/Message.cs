namespace Messages
{
    public enum MessageType
    {
        ClientConnectionMessage = 0,
        Ready = 1,
        Guess = 2,
        Timeout = 3,
        ServerConnectionSuccess = 4,
        ServerConnectionFailure = 5,
        PlayerList = 6,
        GameStarted = 7,
        GameStatus = 8,
        GuessResult = 9,
        GameResult = 10,
    }

    public abstract class Message
    {
        public MessageType MessageType { get; protected set; }

        public abstract byte[] Serialize();
    }

    public class MessageFactory
    {
        public static Message CreateMessage(byte[] message)
        {
            byte type = message[0];
            return type switch
            {
                (byte)MessageType.ClientConnectionMessage => new ClientConnectionMessage(message),
                (byte)MessageType.Ready => new ReadyMessage(message),
                (byte)MessageType.Guess => new GuessMessage(message),
                (byte)MessageType.Timeout => new TimeoutMessage(message),
                (byte)MessageType.ServerConnectionSuccess => new ServerConnectionSuccessMessage(message),
                (byte)MessageType.ServerConnectionFailure => new ServerConnectionFailureMessage(message),
                (byte)MessageType.PlayerList => new PlayerListMessage(message),
                (byte)MessageType.GameStarted => new GameStartedMessage(message),
                (byte)MessageType.GameStatus => new GameStatusMessage(message),
                (byte)MessageType.GuessResult => new GuessResultMessage(message),
                (byte)MessageType.GameResult => new GameResultMessage(message),
                _ => throw new Exception("Invalid message type"),
            };
        }
    }
}

