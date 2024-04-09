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
        GameOver
    }

    public enum ServerPlayerState
    {
        NotReady,
        Ready,
        InGame,
        Guessing,
        GameOver,
        Disconnected
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
        static readonly Database database = new("database.txt");

        private readonly uint MaxPlayers = 10;

        public ServerState ServerState { get; private set; } = ServerState.WaitingForPlayers;

        private List<ServerPlayerInfo> m_Players = [];

        public uint CurrentPlayerTurn { get; private set; } = 0;

        public uint CurrentGameTurn { get; private set; } = 0;

        internal string m_KeyWord = null!;

        public string m_Hint = null!;

        public string Revealed { get; private set; } = null!;

        public List<ServerPlayerInfo> Players => m_Players;

        public string KeyWord => m_KeyWord;

        public string Hint => m_Hint;

        public ServerHandler()
        {
        }

        public List<Tuple<string, bool>> GetPlayerReadyList()
        {
            return m_Players.Select(player => new Tuple<string, bool>(player.Username, player.State == ServerPlayerState.Ready)).ToList();
        }

        public List<PlayerInfo> GetPlayerInfoList()
        {
            return m_Players.Select(player => new PlayerInfo(player.Username, (ushort)player.Score, (PlayerState)player.State)).ToList();
        }

        public ServerPlayerInfo? GetPlayer(string playerId)
        {
            return m_Players.Find(player => player.PlayerId == playerId);
        }

        public void Disconnect(string playerId)
        {
            for (int i = 0; i < m_Players.Count; i++)
            {
                if (m_Players[i].PlayerId == playerId)
                {
                    m_Players[i].State = ServerPlayerState.Disconnected;
                    break;
                }
            }
        }

        public void StartGame()
        {
            if (m_Players.Count < 2)
            {
                throw new InvalidOperationException("Not enough players");
            }
            ServerState = ServerState.GameInProgress;
            KeywordDescription kw = database.GetRandomKeyword();
            m_KeyWord = kw.Keyword;
            m_Hint = kw.Description;
            CurrentPlayerTurn = 0;
            CurrentGameTurn = 1;
            Revealed = new string(m_KeyWord.Select(c => '_').ToArray());
        }

        public void NextTurn()
        {
            while (m_Players[(int)CurrentPlayerTurn].State
                == ServerPlayerState.GameOver
                || m_Players[(int)CurrentPlayerTurn].State
                == ServerPlayerState.Disconnected
            )
            {
                CurrentPlayerTurn = (uint)((CurrentPlayerTurn + 1) % m_Players.Count);
            }
            if (CurrentPlayerTurn == 0)
            {
                CurrentGameTurn++;
                if (CurrentGameTurn > 5)
                {
                    FinishGame();
                }
            }
        }

        public void FinishGame()
        {
            ServerState = ServerState.GameOver;
        }

        public void ResetGame()
        {
            ServerState = ServerState.WaitingForPlayers;
            m_Players = [];
            m_KeyWord = null!;
            m_Hint = null!;
            Revealed = null!;
        }

        public List<PlayerResult> GetResults()
        {
            var results = m_Players.Select(player => new PlayerResult(player.Username, (ushort)player.Score, 0)).ToList();
            results.Sort((a, b) => b.Score - a.Score);
            for (int i = 0; i < results.Count; i++)
            {
                results[i].Rank = (byte)(i + 1);
            }
            return results;
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

            if (ServerState == ServerState.GameInProgress)
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
            if (ServerState != ServerState.WaitingForPlayers)
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

        public GuessResult Guess(string playerId, GuessType type, string guess)
        {
            if (ServerState != ServerState.GameInProgress)
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
                    if (Revealed.Contains(guess))
                    {
                        return GuessResult.Duplicate;
                    }
                    Revealed = new string(m_KeyWord.Select((c, i) => c == guess[0] ? c : Revealed[i]).ToArray());
                    if (Revealed == m_KeyWord)
                    {
                        FinishGame();
                        return GuessResult.Correct;
                    }
                    GetPlayer(playerId)!.Score++;
                    return GuessResult.Correct;
                }
                return GuessResult.Incorrect;
            }
            else if (type == GuessType.Keyword)
            {
                if (guess == m_KeyWord)
                {
                    ServerState = ServerState.WaitingForPlayers;
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
    }
}