using System.Text;

namespace Messages
{
    public enum ErrorCode
    {
        InvalidName = 0,
        GameInProgress = 1,
        ServerIsFull = 2,
        InternalServerError = 3,
        NameAlreadyTaken = 4,
    }

    public class ServerConnectionFailureMessage : Message
    {
        public ErrorCode ErrorCode { get; set; }

        public string ErrorMessage { get; set; }

        public ServerConnectionFailureMessage(ErrorCode errorCode, string errorMessage)
        {
            MessageType = MessageType.ServerConnectionFailure;
            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
        }

        public ServerConnectionFailureMessage(byte[] message)
        {
            MessageType = MessageType.ServerConnectionFailure;
            ErrorCode = (ErrorCode)message[1];
            ErrorMessage = Encoding.UTF8.GetString(message, 2, message.Length - 2);
        }

        public override byte[] Serialize()
        {
            byte[] errorMessageBytes = Encoding.UTF8.GetBytes(ErrorMessage);
            byte[] message = new byte[errorMessageBytes.Length + 2];
            message[0] = (byte)MessageType.ServerConnectionFailure;
            message[1] = (byte)ErrorCode;
            errorMessageBytes.CopyTo(message, 2);
            return message;
        }
    }
}