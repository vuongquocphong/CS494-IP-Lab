namespace Messages
{
    public class ServerConnectionSuccessMessage : Message
    {
        public ServerConnectionSuccessMessage()
        {
            MessageType = MessageType.ServerConnectionSuccess;
        }

        public ServerConnectionSuccessMessage(byte[] _)
        {
            MessageType = MessageType.ServerConnectionSuccess;
        }

        public override byte[] Serialize()
        {
            return [(byte)MessageType.ServerConnectionSuccess];
        }
    }
}