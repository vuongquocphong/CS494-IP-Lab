using GameComponents;
using Messages;

namespace Mediator
{
    public interface IMediator
    {
        abstract void Notify(object sender, Message msg);
    }
}