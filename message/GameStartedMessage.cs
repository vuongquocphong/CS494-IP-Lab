using System.Text;

namespace Messages
{
    public class GameStartedMessage : Message
    {
        public string KeyWord { get; private set; }

        public string Hint { get; private set; }

        public GameStartedMessage(string keyWord, string hint)
        {
            MessageType = MessageType.GameStarted;
            KeyWord = keyWord;
            Hint = hint;
        }

        public GameStartedMessage(byte[] message)
        {
            MessageType = MessageType.GameStarted;
            byte keyWordLength = message[1];
            byte[] kw = message[2..(2 + keyWordLength)];
            KeyWord = Encoding.UTF8.GetString(kw);
            byte[] hintLengthBytes = message[(2 + keyWordLength)..(4 + keyWordLength)];
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(hintLengthBytes);
            }
            ushort hintLength = BitConverter.ToUInt16(hintLengthBytes);
            Hint = Encoding.UTF8.GetString(message, 4 + keyWordLength, hintLength);
        }

        public override byte[] Serialize()
        {
            byte[] keyWordBytes = Encoding.UTF8.GetBytes(KeyWord);
            byte[] hintBytes = Encoding.UTF8.GetBytes(Hint);
            byte type = (byte)MessageType.GameStarted;
            byte keyWordLength = (byte)keyWordBytes.Length;
            byte[] hintLength = BitConverter.GetBytes((ushort)hintBytes.Length);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(hintLength);
            }
            byte[] message = [type, keyWordLength, ..keyWordBytes, ..hintLength, ..hintBytes];
            return message;
        }
    }
}