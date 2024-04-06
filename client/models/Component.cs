using Mediator;

namespace GameComponents
{
    public abstract class Component(IMediator MessagePasser)
    {
        protected IMediator MessagePasser = MessagePasser;
    }
}