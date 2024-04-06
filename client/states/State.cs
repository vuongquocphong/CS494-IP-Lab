using GameComponents;

namespace StateManager {
    public abstract class State {
        protected GameManager _gameManager = null!;
        public void SetContext(GameManager gameManager) {
            this._gameManager = gameManager;
        }
        public abstract void Handle(GameManager gameManager);
    }
}