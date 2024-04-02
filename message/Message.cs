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
        GameStatus = 7,
        GuessResult = 8,
        GameResult = 9,
    }

    public abstract class Message
    {
        protected MessageType MessageType { get; set; }

        public abstract byte[] Serialize();
    }

    public class MessageFactory 
    {
        public static Message CreateMessage(byte[] message)
        {
            int type = message[0];
            return type switch
            {
                (int)MessageType.ClientConnectionMessage => new ClientConnectionMessage(message),
                (int)MessageType.Ready => new ReadyMessage(message),
                (int)MessageType.Guess => new GuessMessage(message),
                (int)MessageType.Timeout => new TimeoutMessage(message),
                (int)MessageType.ServerConnectionSuccess => new ServerConnectionSuccessMessage(message),
                (int)MessageType.ServerConnectionFailure => new ServerConnectionFailureMessage(message),
                (int)MessageType.PlayerList => new PlayerListMessage(message),
                (int)MessageType.GameStatus => new GameStatusMessage(message),
                (int)MessageType.GuessResult => new GuessResultMessage(message),
                (int)MessageType.GameResult => new GameResultMessage(message),
                _ => throw new Exception("Invalid message type"),
            };
        }
    }
}

