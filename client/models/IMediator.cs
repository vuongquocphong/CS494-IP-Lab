namespace Mediator
{
    enum Event
    {
        CONNECT,
        READY,
        GUESS,
        TIMEOUT,
        MOCKRCV,
        TCPRCV
    }
    interface IMediator
    {   
        abstract void Notify(object sender, Event ev);
    }
}