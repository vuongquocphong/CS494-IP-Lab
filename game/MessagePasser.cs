using GameComponents;
using Messages;
using NetworkClient;
using Mediator;
using Godot;

namespace Mediator
{
    public class MessagePasser: IMediator
    {
        private GameManager gameManager;
        private INetworkClient networkClient;

        public MessagePasser(GameManager gameManager, INetworkClient networkClient) {
            this.gameManager = gameManager;
            this.networkClient = networkClient;
            this.gameManager.MediatorComp = (IMediator) this;
            this.networkClient.Mediator = (IMediator) this;
        }

        public void Notify(object sender, Message msg)
        {
            switch (sender) 
            {
                case GameManager:
                    ReactOnGameManager(msg);
                    break;
                case INetworkClient:
                    ReactOnNetworkClient(msg);
                    break;
                default:
                    GD.Print(sender.GetType().ToString() + " is not a valid sender");
                    break;
            }
        }

        private void ReactOnGameManager(Message msg)
        {
            networkClient.Send(msg.Serialize());
        }
        private void ReactOnNetworkClient(Message msg)
        {
            GD.Print(msg.MessageType);
            switch (msg.MessageType) {
                case MessageType.ServerConnectionFailure:
                    gameManager.ConnectFail((ServerConnectionFailureMessage) msg);
                    break;
                case MessageType.ServerConnectionSuccess:
                    gameManager.ConnectSuccess();
                    break;
                case MessageType.PlayerList:
                    List<Tuple<string, bool>> players = ((PlayerListMessage) msg).Players;
                    gameManager.UpdatePlayerList(players);
                    break;
                case MessageType.GameStarted:
                    string KeyWord = ((GameStartedMessage) msg).KeyWord;
                    string Hint = ((GameStartedMessage) msg).Hint;
                    gameManager.StartGame(KeyWord, Hint);
                    break;
                case MessageType.GameStatus:
                    gameManager.UpdateGameStatus((GameStatusMessage) msg);
                    break;
                case MessageType.GameResult:
                    List<PlayerResult> playerResults = ((GameResultMessage) msg).Results;
                    gameManager.UpdateGameResult(playerResults);
                    break;
                case MessageType.GuessResult:
                    gameManager.NotifyResult((GuessResultMessage)msg);
                    break;
                default:
                    break;
            }
        }
    }
}