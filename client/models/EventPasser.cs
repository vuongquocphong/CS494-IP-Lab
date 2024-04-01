using Mediator;
using NetworkClient;

namespace EventPasser
{
    class EventPasser: Mediator.IMediator
    {
        GameManager.GameManager GameManagerComp;
        INetworkClient NetworkClientComp;
        public EventPasser(GameManager.GameManager gameManager, INetworkClient networkClient)
        {
            GameManagerComp = gameManager;
            NetworkClientComp = networkClient;
        }

        public void Notify(object sender, Event ev)
        {
            if (sender is GameManager.GameManager)
            {
                ReactOnGameManager(ev);
            }
            else if (sender is INetworkClient)
            {
                ReactOnNetworkClient(ev);
            }
        }
        private void ReactOnGameManager(Event ev)
        {
            // React on GameManager
        }
        private void ReactOnNetworkClient(Event ev)
        {
            // React on NetworkClient
        }
    }
}