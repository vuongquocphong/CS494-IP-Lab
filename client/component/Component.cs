using Mediator;

namespace Components
{
    public interface IComponent
    {
        IMediator Mediator { get; set; }
    }
}