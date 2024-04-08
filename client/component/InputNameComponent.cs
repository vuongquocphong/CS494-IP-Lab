using Mediator;

namespace Components
{
    public class InputNameComponent : IComponent
    {
        public InputNamePanel ComponentNode { get; set; }
        private IMediator _EventPasser;
        public String PlayerName { get; set; } = "";
        public IMediator Mediator { get => _EventPasser; set => _EventPasser = value; }

        public InputNameComponent(IMediator MediatorComp)
        {
            ComponentNode = new InputNamePanel();
            _EventPasser = MediatorComp;
        }
        public void OnPlayButtonPressed(string Name)
        {
            PlayerName = Name;
            _EventPasser.Notify(this, Event.REQUEST_CONNECT);
        }
    }
}