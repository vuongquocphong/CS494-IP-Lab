using Godot;
using Mediator;
using NetworkClient;

namespace EventPasser
{
    class EventPasser: Mediator.IMediator
    {
        // Model classes
        GameManager.GameManager GameManagerComp;
        INetworkClient NetworkClientComp;


        // Scene classes
        InputNamePanel InputNamePanelComp;
        WaitingPanel WaitingPanelComp;
        
        public EventPasser(GameManager.GameManager gameManager, INetworkClient networkClient, InputNamePanel inputNamePanel, WaitingPanel waitingPanel)
        {
            GameManagerComp = gameManager;
            NetworkClientComp = networkClient;
            InputNamePanelComp = inputNamePanel;
            WaitingPanelComp = waitingPanel;
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
            else if (sender is InputNamePanel){
                ReactOnInputNamePanel(ev);
            }
            else if (sender is WaitingPanel){
                ReactOnWaitingPanel(ev);
            }
        }

        private void ReactOnWaitingPanel(Event ev)
        {
            throw new NotImplementedException();
        }


        private void ReactOnInputNamePanel(Event ev)
        {
            switch(ev)
            {
                case Event.ADDPLAYER:
                    var name = InputNamePanelComp.GetName();
                    GameManagerComp.AddPlayer(name);
                    break;
                case Event.CONNECT:

                    break;
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