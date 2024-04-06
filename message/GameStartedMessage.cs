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
            MessageType = (MessageType)message[0];
            byte keyWordLength = message[1];
            KeyWord = System.Text.Encoding.UTF8.GetString(message, 2, keyWordLength);
            ushort hintLength = BitConverter.ToUInt16(message, 2 + keyWordLength);
            Hint = System.Text.Encoding.UTF8.GetString(message, 4 + keyWordLength, hintLength);
        }

        public override byte[] Serialize()
        {
            byte[] keyWordBytes = System.Text.Encoding.UTF8.GetBytes(KeyWord);
            byte[] hintBytes = System.Text.Encoding.UTF8.GetBytes(Hint);
            byte[] message = new byte[4 + keyWordBytes.Length + hintBytes.Length];
            message[0] = (byte)MessageType.GameStarted;
            message[1] = (byte)keyWordBytes.Length;
            keyWordBytes.CopyTo(message, 2);
            byte[] hintLength = BitConverter.GetBytes((ushort)hintBytes.Length);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(hintLength);
            }
            hintLength.CopyTo(message, 2 + keyWordBytes.Length);
            hintBytes.CopyTo(message, 4 + keyWordBytes.Length);
            return message;
        }
    }
}