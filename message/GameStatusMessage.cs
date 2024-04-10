using System.Text;

namespace Messages
{
    public enum PlayerState
    {
        Playing = 0,
        Lost = 1,
        Disconnected = 2,
        Won = 3,
    }

    public class PlayerInfo(string name, ushort score, PlayerState state)
    {
        public string Name { get; set; } = name;
        public ushort Score { get; set; } = score;
        public PlayerState State { get; set; } = state;

    }

    public class GameStatusMessage : Message
    {
        public int PlayerCount { get; set; }
        public int GameTurn { get; set; }
        public int CurrentTurn { get; set; }
        public int KeywordLength { get; set; }
        public string Keyword { get; set; }

        public List<PlayerInfo> PlayersList { get; set; } = [];

        public GameStatusMessage(int playerCount, int gameTurn, int currentTurn, string keyword, List<PlayerInfo> playersList)
        {
            MessageType = MessageType.GameStatus;
            PlayerCount = playerCount;
            GameTurn = gameTurn;
            CurrentTurn = currentTurn;
            KeywordLength = keyword.Length;
            Keyword = keyword;
            PlayersList = playersList;
        }

        public GameStatusMessage(byte[] message)
        {
            MessageType = MessageType.GameStatus;
            PlayerCount = message[1];
            GameTurn = message[2];
            CurrentTurn = message[3];
            KeywordLength = message[4];
            Keyword = Encoding.UTF8.GetString(message, 5, KeywordLength);
            int ptr = 5 + KeywordLength;
            while (ptr < message.Length)
            {

                int offset = ptr;
                byte nameLength = message[offset];
                string name = Encoding.UTF8.GetString(message, 1, nameLength);
                offset += nameLength + 1;
                byte[] byteScore = message[offset..(offset + 2)];
                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(byteScore);
                }
                ushort score = BitConverter.ToUInt16(byteScore);
                offset += 2;
                PlayerState state = (PlayerState)message[offset];
                offset += 1;
                PlayersList.Add(new PlayerInfo(name, score, state));
                ptr = offset;
            }
        }

        public override byte[] Serialize()
        {
            List<byte> message =
            [
                (byte)MessageType,
                (byte)PlayerCount,
                (byte)GameTurn,
                (byte)CurrentTurn,
                (byte)KeywordLength,
                .. Encoding.UTF8.GetBytes(Keyword),
            ];
            foreach (var player in PlayersList)
            {
                message.Add((byte)player.Name.Length);
                message.AddRange(Encoding.UTF8.GetBytes(player.Name));
                byte[] score = BitConverter.GetBytes(player.Score);
                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(score);
                }
                message.AddRange(score);
                message.Add((byte)player.State);
            }
            return [.. message];
        }
    }
}