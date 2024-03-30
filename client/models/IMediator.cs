namespace Mediator
{
    interface IMediator
    {
        abstract void Notify(object sender, string ev);
    }
}