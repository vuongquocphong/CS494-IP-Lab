using System.Text;

namespace Messages
{
    public enum GuessType
    {
        Character = 0,
        Keyword = 1
    }

    public class GuessMessage : Message
    {
        public GuessType GuessType { get; set; }

        public string Guess { get; set; }

        public GuessMessage(byte[] message)
        {
            MessageType = MessageType.Guess;
            GuessType = (GuessType)message[1];
            Guess = Encoding.UTF8.GetString(message, 2, message.Length - 2);
        }

        public override byte[] Serialize()
        {
            byte[] guessBytes = Encoding.UTF8.GetBytes(Guess);
            byte[] message = new byte[guessBytes.Length + 2];
            message[0] = (byte)MessageType.Guess;
            message[1] = (byte)GuessType;
            guessBytes.CopyTo(message, 2);
            return message;
        }
    }
}