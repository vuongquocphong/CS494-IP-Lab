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
        [Signal]
        public delegate void StartGameReceiveEventHandler();
        [Signal]
        public delegate void GameResultReceiveEventHandler();
        [Signal]
        public delegate void GuessResultReceiveEventHandler(string name, string PlayerName);
        [Signal]
        public delegate void GameStatusReceiveEventHandler();
        static private GameManager instance = null!;
        public string LocalPlayerName { get; set; } = "";
        public string KeyWord { get; set; } = "";
        public string Hint { get; set; } = "";
        public int NumberOfPlayers { get; set; } = 0;
        public List<PlayerInfo> PlayersList { get; set; } = [];
        public bool GameState { get; set; } = false;
        public PlayerInfo CurrentPlayer { get; set; } = null!;
        public IMediator MediatorComp { get; set; } = null!;
        public Messages.GuessResult guessResult { get; set; } = new GuessResult();
        public void RequestConnect(string name)
        {
            GD.Print("RequestConnect");
            LocalPlayerName = name;
            GD.Print(LocalPlayerName);
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
            GameState = false;
            CurrentPlayer = null!;
        }
        public void ConnectSuccess()
        {
            CallDeferred("emit_signal", "ConnectionSuccess");
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

            PlayersList = newPlayersList;
            // Emit signal to update UI
            CallDeferred("emit_signal", "PlayerListUpdate");
        }
        public void SendReady(bool ready)
        {
            ReadyMessage msg = new(ready);
            MediatorComp.Notify(this, msg);
        }


        public void StartGame(string keyWord, string hint)
        {
            KeyWord = keyWord;
            Hint = hint;
            GameState = true;
            CallDeferred("emit_signal", "StartGameReceive");
        }
        public void Guess(bool guessMode, string guess) {
            // Send message to server
            // to check guess
            // Notify mediator
            Messages.GuessType guessType = guessMode switch
            {
                true => Messages.GuessType.Character,
                false => Messages.GuessType.Keyword
            };
            GuessMessage msg = new GuessMessage(guessType, guess);
            MediatorComp.Notify(this, msg);
        }
        public void TimeOut()
        {
            TimeoutMessage msg = new();
            MediatorComp.Notify(this, msg);
        }

        public void UpdateGameResult(List<PlayerResult> results)
        {
            PlayersList.Clear();
            foreach (PlayerResult result in results)
            {
                PlayersList.Add(new PlayerInfo(result.PlayerName) {
                    Point = result.Score,
                });
            }
            CallDeferred("emit_signal", "GameResultReceive");
        }

        internal void AddPlayer(string name)
        {
            PlayersList.Add(new PlayerInfo(name));
        }

        public void UpdateGameStatus(GameStatusMessage msg)
        {
            NumberOfPlayers = msg.PlayerCount;
            List<PlayerInfo> newPlayersList = new List<PlayerInfo>();
            int index = 0;
            KeyWord = msg.Keyword;
            foreach (Messages.PlayerInfo player in msg.PlayersList)
            {
                PlayerInfo NewPlayer = new(player.Name);
                NewPlayer.Point = player.Score;
                NewPlayer.State = player.State switch
                {
                    Messages.PlayerState.Playing => PlayerState.Playing,
                    Messages.PlayerState.Lost => PlayerState.Lost,
                    Messages.PlayerState.Disconnected => PlayerState.Disconnected,
                    _ => throw new InvalidEnumArgumentException()
                };
                GD.Print(NewPlayer.Name.Length);
                GD.Print("New player name: " + NewPlayer.Name);
                newPlayersList.Add(NewPlayer);
                if (index == msg.CurrentTurn){
                    CurrentPlayer = NewPlayer;
                }
                GD.Print(CurrentPlayer.Name.Length);
                GD.Print("Current player name: " + CurrentPlayer.Name);
                index++;
            }
            PlayersList = newPlayersList;
            CallDeferred(MethodName.EmitSignal, "GameStatusReceive");
        }

        public void NotifyResult(GuessResultMessage msg)
        {
            guessResult = msg.Result;
            CallDeferred(MethodName.EmitSignal, "GuessResultReceive", msg.Guess, msg.PlayerName);
        }
    }
}