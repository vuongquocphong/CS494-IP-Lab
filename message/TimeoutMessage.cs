namespace Messages {
    public class TimeoutMessage : Message {
        public TimeoutMessage() {
            MessageType = MessageType.Timeout;
        }

        public TimeoutMessage(byte[] _) {
            MessageType = MessageType.Timeout;
        }

        public override byte[] Serialize() {
            return [(byte)MessageType.Timeout];
        }
    }
}