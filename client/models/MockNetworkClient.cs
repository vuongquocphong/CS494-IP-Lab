namespace NetworkClient
{
    class MockNetworkClient: INetworkClient
    {
        public MockNetworkClient(Mediator.IMediator mediator): base(mediator)
        {
        }
        public void Send()
        {
            // Send data to server
        }
        public void Receive()
        {
            // Receive data from server
        }
    }
}