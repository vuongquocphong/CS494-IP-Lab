using Mediator;

namespace NetworkClient
{
    interface INetworkClient {
        public IMediator Mediator { get; set; }
        abstract void Send(byte[] message);
        abstract void Receive();
    }
}