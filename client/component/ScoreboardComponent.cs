using Mediator;

namespace Component
{
    public class ScoreboardComponent
    {
        public ScoreboardPanel ComponentNode;
        private IMediator MediatorComp;
        public ScoreboardComponent(IMediator MediatorComp){
            ComponentNode = new ScoreboardPanel();
            this.MediatorComp = MediatorComp;
        }
    }
}