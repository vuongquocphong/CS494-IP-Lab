using GameComponents;
using Messages;
using NetworkClient;

namespace Mediator
{
    public class MessagePasser : IMediator
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
            throw new NotImplementedException();
        }

        private void ReactOnGameManager(Message msg)
        {
            // React on GameManager
        }
        private void ReactOnNetworkClient(Message msg)
        {
            // React on NetworkClient
        }
    }
}