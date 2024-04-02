namespace Messages {
    public class TimeoutMessage : Message {
        public TimeoutMessage(byte[] _) {
            MessageType = MessageType.Timeout;
        }

        public override byte[] Serialize() {
            return [(byte)MessageType.Timeout];
        }
    }
}