using Messages;

namespace GameServer
{

    public enum AddPlayerResult
    {
        Success,
        ServerIsFull,
        NameAlreadyTaken,
        GameInProgress,
        InvalidName,
        InternalServerError
    }

    public enum ServerState
    {
        WaitingForPlayers,
        GameInProgress,
    }

    public enum ServerPlayerState
    {
        NotReady,
        Ready,
        InGame,
        Guessing,
        GameOver
    }

    public class ServerPlayerInfo(string address, string username, int score, ServerPlayerState state)
    {
        private readonly string m_address = address;
        public string Username { get; set; } = username;
        public int Score { get; set; } = score;
        public ServerPlayerState State { get; set; } = state;

        public string PlayerId
        {
            get { return m_address; }
        }
    }

    public class ServerHandler
    {
        private readonly uint MaxPlayers = 10;

        private ServerState m_ServerState = ServerState.WaitingForPlayers;

        private readonly MessageFactory m_MessageFactory = new();

        private List<ServerPlayerInfo> m_Players = [];

        private string m_KeyWord = null!;

        private string m_Description = null!;

        private string m_Revealed = null!;

        public ServerHandler()
        {
        }

        public ServerPlayerInfo? GetPlayer(string playerId)
        {
            return m_Players.Find(player => player.PlayerId == playerId);
        }

        public void StartGame()
        {
            if (m_Players.Count < 2)
            {
                throw new InvalidOperationException("Not enough players");
            }
            m_ServerState = ServerState.GameInProgress;
            m_KeyWord = "test";
            m_Description = "test description";
            m_Revealed = new string(m_KeyWord.Select(c => '_').ToArray());
        }

        public AddPlayerResult AddPlayer(string address, string username)
        {
            if (m_Players.Count >= MaxPlayers)
            {
                return AddPlayerResult.ServerIsFull;
            }

            if (m_Players.Any(player => player.Username == username))
            {
                return AddPlayerResult.NameAlreadyTaken;
            }

            if (m_ServerState == ServerState.GameInProgress)
            {
                return AddPlayerResult.GameInProgress;
            }

            if (username.Length == 0 || username.Any(c => !char.IsLetterOrDigit(c)))
            {
                return AddPlayerResult.InvalidName;
            }

            m_Players.Add(new ServerPlayerInfo(address, username, 0, ServerPlayerState.NotReady));
            return AddPlayerResult.Success;
        }

        public void Ready(string playerId, bool ready)
        {
            if (m_ServerState != ServerState.WaitingForPlayers)
            {
                throw new InvalidOperationException("Game is not waiting for players");
            }
            for (int i = 0; i < m_Players.Count; i++)
            {
                if (m_Players[i].PlayerId == playerId)
                {
                    m_Players[i].State = ready ? ServerPlayerState.Ready : ServerPlayerState.NotReady;
                    break;
                }
            }
        }

        public GuessResult Guess(string playerId, GuessType type, string guess) {
            if (m_ServerState != ServerState.GameInProgress)
            {
                throw new InvalidOperationException("Game is not in progress");
            }
            if (type == GuessType.Character)
            {
                if (guess.Length != 1 || !char.IsLetter(guess[0]))
                {
                    return GuessResult.Invalid;
                }
                if (m_KeyWord.Contains(guess))
                {
                    m_Revealed = new string(m_KeyWord.Select(c => c == guess[0] ? c : '_').ToArray());
                    if (m_Revealed == m_KeyWord)
                    {
                        // TODO: Finish game
                        return GuessResult.Correct;
                    }
                    return GuessResult.Correct;
                }
                return GuessResult.Incorrect;
            }
            else if (type == GuessType.Keyword)
            {
                if (guess == m_KeyWord)
                {
                    m_ServerState = ServerState.WaitingForPlayers;
                    // TODO: Finish game
                    return GuessResult.Correct;
                }
                for (int i = 0; i < m_Players.Count; i++)
                {
                    if (m_Players[i].PlayerId == playerId)
                    {
                        m_Players[i].State = ServerPlayerState.GameOver;
                        break;
                    }
                }
                return GuessResult.Incorrect;
            }
            else
            {
                return GuessResult.Invalid;
            }
        }

        public void Timeout(string playerId)
        {
            if (m_ServerState != ServerState.GameInProgress)
            {
                throw new InvalidOperationException("Game is not in progress");
            }
            for (int i = 0; i < m_Players.Count; i++)
            {
                if (m_Players[i].PlayerId == playerId)
                {
                    // TODO: Next turn
                    m_Players[i].State = ServerPlayerState.GameOver;
                    break;
                }
            }
        }
    }
}