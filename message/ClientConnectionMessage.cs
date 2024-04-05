using System.Text;

namespace Messages
{

    public class ClientConnectionMessage : Message
    {
        public string Username { get; set; }

        public ClientConnectionMessage(string username)
        {
            MessageType = MessageType.ClientConnectionMessage;
            Username = username;
        }

        public ClientConnectionMessage(byte[] message)
        {
            MessageType = MessageType.ClientConnectionMessage;
            Username = Encoding.UTF8.GetString(message, 1, message.Length - 1);
        }

        public override byte[] Serialize()
        {
            byte[] usernameBytes = Encoding.UTF8.GetBytes(Username);
            byte[] message = new byte[usernameBytes.Length + 1];
            message[0] = (byte)MessageType.ClientConnectionMessage;
            usernameBytes.CopyTo(message, 1);
            return message;
        }
    }

}