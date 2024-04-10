using Godot;
using Mediator;
using Messages;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;

namespace GameComponents
{
    public partial class GameManager : Node
    {
        [Signal]
        public delegate void PlayButtonPressedEventHandler(string name);
        [Signal]
        public delegate void ConnectionSuccessEventHandler();
        [Signal]
        public delegate void ConnectionFailEventHandler(string error);
        [Signal]
        public delegate void PlayerListUpdateEventHandler();
        [Signal]
        public delegate void BackFromWaitingToInputNameEventHandler();
        [Signal]
        public delegate void BackFromIngameToInputNameEventHandler();
        [Signal]
        public delegate void BackFromScoreBoardToInputNameEventHandler();
        [Signal]
        public delegate void BackFromScoreBoardToWaitingEventHandler();
        [Signal]
        public delegate void ReadyButtonPressedEventHandler();
        static private GameManager instance = null!;
        public string LocalPlayerName { get; set; } = "";
        public string KeyWord { get; set; } = "";
        public string Hint { get; set; } = "";
        public int NumberOfPlayers { get; set; } = 0;
        public List<PlayerInfo> PlayersList { get; set; } = [];
        public int NumberOfTurns { get; set; } = 0;
        public bool GameState { get; set; } = false;
        public PlayerInfo CurrentPlayer { get; set; } = null!;
        public IMediator MediatorComp { get; set; } = null!;
        public void RequestConnect(string name)
        {
            LocalPlayerName = name;
            ClientConnectionMessage msg = new(name);
            MediatorComp.Notify(this, msg);
        }

        private GameManager() { }

        static public GameManager GetInstance()
        {
            instance ??= new GameManager();
            return instance;
        }

        public void Reset()
        {
            LocalPlayerName = "";
            KeyWord = "";
            NumberOfPlayers = 0;
            PlayersList.Clear();
            NumberOfTurns = 0;
            GameState = false;
            CurrentPlayer = null!;
        }

        public void ConnectSuccess()
        {
            CallDeferred("emit_signal", "ConnectionSuccess");
        }

        public void ConnectFail(string error)
        {
            CallDeferred("emit_signal", "ConnectionFail", error);
        }
        public void ConnectFail(ServerConnectionFailureMessage msg)
        {
            string ErrorContent;
            ErrorContent =
            msg.ErrorCode switch
            {
                ErrorCode.InvalidName => "Invalid Name",
                ErrorCode.ServerIsFull => "Server Is Full",
                ErrorCode.GameInProgress => "Game In Progress",
                ErrorCode.InternalServerError => "Internal Server Error",
                ErrorCode.NameAlreadyTaken => "Name Already Taken",
                _ => "Unknown Error Code"
            };
            CallDeferred("emit_signal", "ConnectionFail", ErrorContent);
        }

        public void UpdatePlayerList(List<Tuple<string, bool>> players)
        {
            List<PlayerInfo> newPlayersList = new();
            foreach (Tuple<string, bool> player in players)
            {
                PlayerInfo playerInfo = new(player.Item1) {
                    ReadyStatus = player.Item2
                };
                newPlayersList.Add(playerInfo);
            }
            // Sort players list by name
            newPlayersList.Sort((a, b) => a.Name.CompareTo(b.Name));
            PlayersList = newPlayersList;
            // Emit signal to update UI
            CallDeferred("emit_signal", "PlayerListUpdate");
        }

        public void SendReady(bool ready)
        {
            ReadyMessage msg = new(ready);
            MediatorComp.Notify(this, msg);
        }
        public void Receive(Message msg)
        {
            // Get Message type
            // and call appropriate function
        }
        public void PlayerReady()
        {
            // Send message to server
            // to start game
            // Notify mediator
        }
        public void Guess(bool guessMode, string guess) {
            // Send message to server
            // to check guess
            // Notify mediator
            // this.Mediator.Notify(this, Event.GUESS);
        }
        public void TimeOut()
        {
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