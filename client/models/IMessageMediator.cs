using Messages;

namespace MessageMediator{
    public interface IMessageMediator{
        public void Notify(object sender, Message msg);
    }
}