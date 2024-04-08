using GameComponents;
using Messages;
using NetworkClient;
using MessageMediator;

namespace MessageMediator
{
    class MessagePasser(GameManager gameManager, INetworkClient networkClient): IMessageMediator
    {
        private GameManager gameManager = gameManager;
        private INetworkClient networkClient = networkClient;

        public void Notify(object sender, Message msg)
        {
            switch (sender) 
            {
                case GameManager:
                    ReactOnGameManager(msg);
                    break;
                case INetworkClient:
                    ReactOnGameManager(msg);
                    break;
            }
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