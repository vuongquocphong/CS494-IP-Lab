using MessageMediator;

namespace NetworkClient
{
    interface INetworkClient {
        public IMessageMediator Mediator { get; set; }
        abstract void Send(byte[] message);
        abstract void Receive(byte[] message);
    }
}