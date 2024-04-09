using Mediator;

namespace NetworkClient
{
    public interface INetworkClient {
        public IMediator Mediator { get; set; }
        abstract void Send(string message);
        abstract void Receive();
    }
}