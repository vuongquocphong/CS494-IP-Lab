using Godot;
using Mediator;
using Messages;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;

namespace GameComponents
{
    public partial class GameManager: Node
    {   
        [Signal]
        public delegate void ConnectionSuccessEventHandler();
        [Signal]
        public delegate void ConnectionFailEventHandler(string error);
        [Signal]
        public delegate void PlayerListUpdateEventHandler();
        static private GameManager instance = null!;
        public string LocalPlayerName { get; set; } = "";
        public string KeyWord { get; set; } = "";
        public int NumberOfPlayers { get; set; } = 0;
        public List<PlayerInfo> PlayersList { get; set; } = [];
        public int NumberOfTurns { get; set; } = 0;
        public bool GameState { get; set; } = false;
        public PlayerInfo CurrentPlayer { get; set; } = null!;
        public IMediator MediatorComp { get; set; } = null!;

        public void RequestConnect(string name)
        {
            ClientConnectionMessage msg = new ClientConnectionMessage(name);
            MediatorComp.Notify(this, msg);
        }

        private GameManager() {}

        static public GameManager GetInstance()
        {
            instance ??= new GameManager();
            return instance;
        }

        public void ConnectSuccess(Message msg) {
            CallDeferred("emit_signal", "ConnectionSuccess");
        }

        public void ConnectFail(string error) {
            CallDeferred("emit_signal", "ConnectionFail", error);
        }
        public void ConnectFail(ServerConnectionFailureMessage msg) {
            string ErrorContent;
            ErrorContent = 
            msg.ErrorCode switch {
                ErrorCode.InvalidName => "Invalid Name",
                ErrorCode.ServerIsFull => "Server Is Full",
                ErrorCode.GameInProgress => "Game In Progress",
                ErrorCode.InternalServerError => "Internal Server Error",
                ErrorCode.NameAlreadyTaken => "Name Already Taken",
                _ => "Unknown Error Code"
            };
            CallDeferred("emit_signal", "ConnectionFail", ErrorContent);
        }

        public void UpdatePlayerList(List<Tuple<string, bool>> players) {
            foreach (Tuple<string, bool> player in players) {
                if (PlayersList.Find(p => p.Name == player.Item1) == null) {
                    PlayerInfo tmp = new(player.Item1)
                    {
                        ReadyStatus = player.Item2
                    };
                    PlayersList.Add(tmp);
                }
                else {
                    PlayerInfo tmp = PlayersList.Find(p => p.Name == player.Item1)!;
                    tmp.ReadyStatus = player.Item2;
                }
            }
            // Sort players list by name
            PlayersList.Sort((a, b) => a.Name.CompareTo(b.Name));
            // Emit signal to update UI
            CallDeferred("emit_signal", "PlayerListUpdate");
        }
        public void Receive(Message msg) {
            // Get Message type
            // and call appropriate function
        }
        public void PlayerReady() {
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

        public void TransitionTo()
        {
        }

        public void ProcessMessage(Message msg)
        {
            // Process message from server
            // and update game state
            
        }
    }
}