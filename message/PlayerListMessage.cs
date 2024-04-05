using System.Text;

namespace Messages
{
    public class PlayerListMessage : Message
    {
        public int PlayerCount { get; set; }

        public List<Tuple<string, bool>> Players { get; set; }

        public PlayerListMessage(int playerCount, List<Tuple<string, bool>> players)
        {
            MessageType = MessageType.PlayerList;
            PlayerCount = playerCount;
            Players = players;
        }

        public PlayerListMessage(byte[] message)
        {
            MessageType = MessageType.PlayerList;
            PlayerCount = message[1];
            Players = [];
            int ptr = 2;
            while (ptr < message.Length)
            {
                int nameLength = message[ptr++];
                string name = Encoding.UTF8.GetString(message, ptr, nameLength);
                ptr += nameLength;
                bool ready = message[ptr++] == 1;
                Players.Add(new Tuple<string, bool>(name, ready));
            }
        }

        public override byte[] Serialize()
        {
            List<byte> message = [(byte)MessageType.PlayerList, (byte)PlayerCount];
            foreach (var player in Players)
            {
                byte[] nameBytes = Encoding.UTF8.GetBytes(player.Item1);
                message.Add((byte)nameBytes.Length);
                message.AddRange(nameBytes);
                message.Add((byte)(player.Item2 ? 1 : 0));
            }
            return [.. message];
        }
    }
}