using System.Text;

namespace Messages
{
    public class PlayerResult(string playerName, ushort score, byte rank)
    {
        public string PlayerName { get; set; } = playerName;

        public ushort Score { get; set; } = score;

        public byte Rank { get; set; } = rank;
    }

    public class GameResultMessage : Message
    {
        public List<PlayerResult> Results { get; set; }

        public GameResultMessage(List<PlayerResult> results)
        {
            MessageType = MessageType.GameResult;
            Results = results;
        }

        public GameResultMessage(byte[] message)
        {
            MessageType = MessageType.GameResult;
            byte playerCount = message[1];
            Results = [];
            int offset = 2;
            for (int i = 0; i < playerCount; i++)
            {
                byte playerNameLength = message[offset++];
                string playerName = Encoding.UTF8.GetString(message, offset, playerNameLength);
                ushort score = BitConverter.ToUInt16(message, offset + playerNameLength);
                byte rank = message[offset + playerNameLength + 2];
                Results.Add(new PlayerResult(playerName, score, rank));
                offset += playerNameLength + 3;
            }
        }

        public override byte[] Serialize()
        {
            byte[] message = [(byte)MessageType, (byte)Results.Count];
            byte[] byteResults = [];
            foreach (PlayerResult result in Results)
            {
                byte[] playerNameBytes = Encoding.UTF8.GetBytes(result.PlayerName);
                byte[] scoreBytes = BitConverter.GetBytes(result.Score);
                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(scoreBytes);
                }
                byte rankBytes = result.Rank;
                byte[] playerResult = [
                    (byte)playerNameBytes.Length,
                    .. playerNameBytes,
                    .. scoreBytes,
                    rankBytes
                ];
                byteResults = [.. byteResults, .. playerResult];
            }
            return message;
        }
    }
}