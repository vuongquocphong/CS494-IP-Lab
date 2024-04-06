namespace Mediator
{
    public enum Event
    {
        CONNECT,
        READY,
        GUESS,
        TIMEOUT,
        MOCKRCV,
        TCPRCV,
        ADDPLAYER,
        
    }
    public interface IMediator
    {   
        abstract void Notify(object sender, Event ev);
    }
}