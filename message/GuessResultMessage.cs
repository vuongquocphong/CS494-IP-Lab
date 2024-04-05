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

        public string Guess { get; set; }

        public GuessResultMessage(byte[] message)
        {
            MessageType = MessageType.GuessResult;
            Result = (GuessResult)message[1];
            GuessType = (GuessType)message[2];
            Guess = Encoding.UTF8.GetString(message, 3, message.Length - 3);
        }

        public override byte[] Serialize()
        {
            byte[] message = new byte[3 + Guess.Length];
            message[0] = (byte)MessageType;
            message[1] = (byte)Result;
            message[2] = (byte)GuessType;
            Encoding.UTF8.GetBytes(Guess).CopyTo(message, 3);
            return message;
        }
    }
}