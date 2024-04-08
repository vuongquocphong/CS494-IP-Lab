using Mediator;

namespace NetworkClient
{
    class TcpNetworkClient(IMediator mediator) : INetworkClient
    {
        public IMediator Mediator { get; set; } = mediator;

        public void Send(byte[] message)
        {
            // Send data to server
        }

        public void Receive()
        {
            // Receive data from server
            // Notify mediator
        }
    }
}