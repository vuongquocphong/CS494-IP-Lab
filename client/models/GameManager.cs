using Mediator;

namespace GameManager
{
    class GameManager {
        public string KeyWord { get; set; }
        public int NumberOfPlayers { get; set; }
        public List<PlayerInfo.PlayerInfo> PlayersList { get; set; }
        public int NumberOfTurns { get; set; }
        public bool GameState { get; set; } // True if game is running
        public PlayerInfo.PlayerInfo CurrentPlayer { get; set; }
        public Player.Player Player { get; set; }
        public IMediator Mediator { get; set; }
        public GameManager(IMediator mediator)
        {
            KeyWord = "";
            NumberOfPlayers = 0;
            PlayersList = new List<PlayerInfo.PlayerInfo>();
            NumberOfTurns = 0;
            GameState = false;
            CurrentPlayer = new PlayerInfo.PlayerInfo();
            Player = new Player.Player();
            Mediator = mediator;
        }
    }
}