using Mediator;

namespace Component
{
    public class IngameComponent
    {
        public IngamePanel ComponentNode;
        private IMediator MediatorComp;
        public IngameComponent(IMediator MediatorComp){
            ComponentNode = new IngamePanel();
            this.MediatorComp = MediatorComp;
        }
    }
}