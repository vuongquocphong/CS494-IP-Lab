using Mediator;

namespace Components
{
    public class IngameComponent(IMediator MediatorComp) : IComponent
    {
        public IngamePanel ComponentNode = new();

        private IMediator _EventPasser = MediatorComp;

        public IMediator Mediator { get => _EventPasser; set => _EventPasser = value; }
    }
}