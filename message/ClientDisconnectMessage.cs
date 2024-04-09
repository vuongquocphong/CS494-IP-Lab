namespace Messages
{
    public class ClientDisconnectMessage : Message
    {
        public ClientDisconnectMessage()
        {
            MessageType = MessageType.ClientDisconnect;
        }

        public ClientDisconnectMessage(byte[] _)
        {
            MessageType = MessageType.ClientDisconnect;
        }

        public override byte[] Serialize()
        {
            return [(byte)MessageType];
        }
    }
}