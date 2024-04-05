
namespace Messages
{
    public class ReadyMessage : Message
    {
        public bool Ready { get; set; }

        public ReadyMessage(bool ready)
        {
            MessageType = MessageType.Ready;
            Ready = ready;
        }

        public ReadyMessage(byte[] message)
        {
            MessageType = MessageType.Ready;
            Ready = message[1] == 1;
        }

        public override byte[] Serialize()
        {
            byte[] message = [(byte)MessageType.Ready, (byte)(Ready ? 1 : 0)];
            return message;
        }
    }
}