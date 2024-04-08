using Mediator;
using MessageMediator;
using Messages;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;

namespace GameComponents
{
    public class GameManager
    {
        public string LocalPlayerName { get; set; } = "";
        public string KeyWord { get; set; } = "";
        public int NumberOfPlayers { get; set; } = 0;
        public List<PlayerInfo> PlayersList { get; set; } = [];
        public int NumberOfTurns { get; set; } = 0;
        public bool GameState { get; set; } = false;
        public PlayerInfo CurrentPlayer { get; set; } = null!;
        public IMediator MediatorComp { get; set; } = null!;

        public IMessageMediator MessageMediatorComp { get; set; } = null!;
        public GameManager(IMediator MediatorComp){
            this.MediatorComp = MediatorComp;
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
    }
}