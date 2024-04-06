using GameComponents;

namespace StateManager {
    public enum Event {
        INPUTNAME,
        BACK,
        ALLREADY,
        QUIT,
        RESTART
    }
    public abstract class State {
        protected GameManager _gameManager = null!;
        public void SetContext(GameManager gameManager) {
            this._gameManager = gameManager;
        }
        public abstract void Handle(Event ev);
        public abstract void OnReady(); 
    }
}