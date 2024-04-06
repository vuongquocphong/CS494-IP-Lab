using System.Text.RegularExpressions;
using GameComponents;

namespace Mediator{
    class EventPasser : IMediator 
    {
        private GameManager GameManagerComp;
        private InputNamePanel InputNamePanelComp;
        private WaitingPanel WaitingPanelComp;
        private IngamePanel IngamePanelComp;
        private ScoreboardPanel ScoreboardPanelComp;

        public void Notify(object sender, Event ev){
            switch (sender) 
            {
                case GameManager:
                    ReactOnGameManager(ev);
                    break;
                case InputNamePanel:
                    ReactOnInputNamePanel(ev);
                    break;
                case WaitingPanel:
                    ReactOnWaitingPanel(ev);
                    break;
                case IngamePanel:
                    ReactOnIngamePanel(ev);
                    break;
                case ScoreboardPanel:
                    ReactOnScoreboardPanel(ev);
                    break;
            }
        }

        private void ReactOnScoreboardPanel(Event ev)
        {
            throw new NotImplementedException();
        }

        private void ReactOnIngamePanel(Event ev)
        {
            throw new NotImplementedException();
        }

        private void ReactOnWaitingPanel(Event ev)
        {
            throw new NotImplementedException();
        }

        private void ReactOnInputNamePanel(Event ev)
        {
            throw new NotImplementedException();
        }

        private void ReactOnGameManager(Event ev)
        {
            throw new NotImplementedException();
        }
    }
}