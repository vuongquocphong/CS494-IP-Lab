using System.Text;

namespace Messages
{
    public enum GuessResult
    {
        Correct = 0,
        Incorrect = 1,
        Duplicate = 2,
        Invalid = 3
    }

    public class GuessResultMessage : Message
    {
        public GuessType GuessType { get; set; }

        public GuessResult Result { get; set; }

        public string PlayerName { get; set; }

        public string Guess { get; set; }

        public GuessResultMessage(GuessResult result, GuessType guessType, string playerName, string guess)
        {
            MessageType = MessageType.GuessResult;
            Result = result;
            GuessType = guessType;
            PlayerName = playerName;
            Guess = guess;
        }

        public GuessResultMessage(byte[] message)
        {
            MessageType = MessageType.GuessResult;
            Result = (GuessResult)message[1];
            GuessType = (GuessType)message[2];
            byte length = message[3];
            PlayerName = Encoding.UTF8.GetString(message, 4, length);
            Guess = Encoding.UTF8.GetString(message, 4 + length, message.Length - 4 - length);
        }

        public override byte[] Serialize()
        {
            byte[] playerNameBytes = Encoding.UTF8.GetBytes(PlayerName);
            byte[] guessBytes = Encoding.UTF8.GetBytes(Guess);
            byte[] message = new byte[4 + playerNameBytes.Length + guessBytes.Length];
            message[0] = (byte)MessageType;
            message[1] = (byte)Result;
            message[2] = (byte)GuessType;
            message[3] = (byte)playerNameBytes.Length;
            playerNameBytes.CopyTo(message, 4);
            guessBytes.CopyTo(message, 4 + playerNameBytes.Length);
            return message;
        }
    }
}