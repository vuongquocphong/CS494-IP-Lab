using Mediator;

namespace NetworkClient
{
    public class TcpNetworkClient: INetworkClient
    {
        public IMediator Mediator { get; set; } = null!;
        public void Send(string message)
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