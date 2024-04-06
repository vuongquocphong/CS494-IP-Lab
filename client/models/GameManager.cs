using Mediator;
using System.Collections.Generic;

namespace GameManager
{
    class GameManager
    {
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
        public void SubmitName(string name) {
            Player.Name = name;
        }
        public void RequestConnect(string name)
        {
            // Send message to server
            // to connect player
            // Notify mediator
            this.Mediator.Notify(this, Event.CONNECT);
        }
        public void Ready() {
            // Send message to server
            // to start game
            // Notify mediator
            this.Mediator.Notify(this, Event.READY);
        }
        public void Guess() {
            // Send message to server
            // to check guess
            // Notify mediator
            this.Mediator.Notify(this, Event.GUESS);
        }
        public void TimeOut() {
            // Send message to server
            // to skip turn
            // Notify mediator
            this.Mediator.Notify(this, Event.TIMEOUT);
        }

        internal void AddPlayer(string name)
        {
            PlayersList.Add(new PlayerInfo.PlayerInfo(name));
        }

    }
}