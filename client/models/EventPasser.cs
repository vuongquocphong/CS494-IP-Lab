using GameComponents;
using Messages;
using NetworkClient;

namespace Mediator
{
    class EventPasser: IMediator
    {
        

        public void Notify(Component sender, Message msg)
        {
            throw new NotImplementedException();
        }

        private void ReactOnGameManager(Message msg)
        {
            // React on GameManager
        }
        private void ReactOnNetworkClient(Message msg)
        {
            // React on NetworkClient
        }
    }
}