using GameComponents;
using Messages;
using NetworkClient;
using Mediator;

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
            }
        }

        private void ReactOnGameManager(Message msg)
        {
            networkClient.Send(msg.Serialize());
        }
        private void ReactOnNetworkClient(Message msg)
        {
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
                default:
                    break;
            }
        }
    }
}