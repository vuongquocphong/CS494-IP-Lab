using Mediator;

namespace Component
{
    public class InputNameComponent
    {
        public InputNamePanel ComponentNode{get; set;}
        private IMediator MediatorComp;

        public String PlayerName{get; set;} = "";
        public InputNameComponent(IMediator MediatorComp){
            ComponentNode = new InputNamePanel();
            this.MediatorComp = MediatorComp;
        }
        public void OnPlayButtonPressed(String Name){
            PlayerName = Name;
            MediatorComp.Notify(this, Event.REQUEST_CONNECT);
        }
    }
}