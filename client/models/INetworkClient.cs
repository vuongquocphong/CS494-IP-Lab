using Mediator;

namespace NetworkClient
{
    interface INetworkClient {
        public IMediator Mediator { get; set; }
        abstract void Send(string message);
        abstract void Receive();
    }
}