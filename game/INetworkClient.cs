using Mediator;

namespace NetworkClient
{
    public interface INetworkClient {
        public IMediator Mediator { get; set; }
        abstract void Send(byte[] message);
        abstract void Receive(byte[] message);
        abstract void Connect();
        abstract void Close();
    }
}