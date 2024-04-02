namespace Messages
{
    public class ServerConnectionSuccessMessage : Message
    {
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