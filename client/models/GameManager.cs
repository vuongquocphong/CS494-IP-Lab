using Mediator;
using StateManager;
using System.Collections.Generic;

namespace GameComponents
{
    public class GameManager(IMediator mediator) : Component(mediator)
    {
        private IState _state = null!;
        public string LocalPlayerName { get; set; } = "";
        public string KeyWord { get; set; } = "";
        public int NumberOfPlayers { get; set; } = 0;
        public List<PlayerInfo> PlayersList { get; set; } = [];
        public int NumberOfTurns { get; set; } = 0;
        public bool GameState { get; set; } = false;
        public PlayerInfo CurrentPlayer { get; set; } = null!;
        public IMediator Mediator { get; set; } = mediator;

        public void RequestConnect(string name)
        {
            // Send message to server
            // to connect player
            // Notify mediator
            // Mediator.Notify(this, Event.CONNECT);
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

        public void TransitionTo(IState state)
        {
            _state = state;
            _state.Handle(this);
        }

    }
}