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

        public void Notify(object sender, string ev)
        {
            if (sender is GameManager.GameManager)
            {
                ReactOnGameManager();
            }
            else if (sender is INetworkClient)
            {
                ReactOnNetworkClient();
            }
        }
        private void ReactOnGameManager()
        {
            // React on GameManager
        }
        private void ReactOnNetworkClient()
        {
            // React on NetworkClient
        }
    }
}