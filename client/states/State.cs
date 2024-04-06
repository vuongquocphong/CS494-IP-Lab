using GameComponents;

namespace StateManager {
    public interface IState {
        public abstract void Handle(GameManager gameManager);
    }
}