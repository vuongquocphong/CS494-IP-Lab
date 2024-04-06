using Mediator;
using Messages;
using StateManager;
using System.Collections.Generic;
using System.ComponentModel;

namespace GameComponents
{
    public class GameManager
    {
        private GameManager instance = null!;
        private State m_State = null!;
        public string LocalPlayerName { get; set; } = "";
        public string KeyWord { get; set; } = "";
        public int NumberOfPlayers { get; set; } = 0;
        public List<PlayerInfo> PlayersList { get; set; } = [];
        public int NumberOfTurns { get; set; } = 0;
        public bool GameState { get; set; } = false;
        public PlayerInfo CurrentPlayer { get; set; } = null!;
        public IMediator MediatorComp { get; set; } = null!;

        // private GameManager(IMediator mediator) : base(mediator) {
        //     this.Mediator = mediator;
        // }

        // public GameManager Init(IMediator mediator) {
        //     instance ??= new GameManager(mediator);
        //     return instance;
        // }

        public GameManager GetInstance() {
            if (instance == null) {
                throw new Exception("Game Manager is not initialized");
            }
            return instance;
        }

        public void RequestConnect(string name)
        {
            // Send message to server
            // to connect player
            // Notify mediator
            // Mediator.Notify(this, Event.CONNECT);
            // this.Mediator.Notify(this, new ClientConnectionMessage(name));
        }

        public void ConnectSuccess() {
            MediatorComp.Notify(this, Mediator.Event.CONNECT_SUCCESS);
        }
        public void ConnectFail(ServerConnectionFailureMessage msg) {
            string InvalidName = "InvalidName";
            InvalidName = 
            msg.ErrorCode switch {
                ErrorCode.InvalidName => "InvalidName",
                ErrorCode.ServerIsFull => "ServerIsFull",
                ErrorCode.GameInProgress => "GameInProgress",
                ErrorCode.InternalServerError => "InternalServerError",
                ErrorCode.NameAlreadyTaken => "NameAlreadyTaken",
                _ => "Unknown"
            };
            
        }
        public void Ready() {
            // Send message to server
            // to start game
            // Notify mediator
            // this.Mediator.Notify(this, Event.READY);
        }
        public void Guess() {
            // Send message to server
            // to check guess
            // Notify mediator
            // this.Mediator.Notify(this, Event.GUESS);
        }
        public void TimeOut() {
            // Send message to server
            // to skip turn
            // Notify mediator
            // this.Mediator.Notify(this, Event.TIMEOUT);
        }

        internal void AddPlayer(string name)
        {
            PlayersList.Add(new PlayerInfo(name));
        }

        public void TransitionTo(State state)
        {
            m_State = state;
            m_State.OnReady();
        }
    }
}