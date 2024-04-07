using Mediator;

namespace Component
{
    public class WaitingComponent
    {
        public WaitingPanel ComponentNode;
        private IMediator MediatorComp;
        public WaitingComponent(IMediator MediatorComp){
            ComponentNode = new WaitingPanel();
            this.MediatorComp = MediatorComp;
        }
        public void SetComponentNode(WaitingPanel ComponentNode){
            
        }
    }
}