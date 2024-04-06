using GameComponents;
using Messages;

namespace Mediator
{
    public interface IMediator
    {
        abstract void Notify(Component sender, Message msg);
    }
}