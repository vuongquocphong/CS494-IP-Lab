using Mediator;

namespace NetworkClient
{
    class TcpNetworkClient: INetworkClient
    {
        public IMediator Mediator { get; set; }
        public TcpNetworkClient(Mediator.IMediator mediator)
        {
            this.Mediator = mediator;
        }
        public void Send(string message)
        {
            // Send data to server
        }
        public void Receive()
        {
            // Receive data from server
            // Notify mediator
            this.Mediator.Notify(this, Event.TCPRCV);
        }
    }
}