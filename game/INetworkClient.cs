using Mediator;
using Messages;

namespace NetworkClient
{
    public interface INetworkClient {
        public IMediator Mediator { get; set; }
        abstract void Send(byte[] message);
        abstract void Receive(Message message);
        abstract void Connect();
        abstract void Close();
    }
}