namespace StateManager {
    public class LoseState : IState {
        public void Handle(GameComponents.GameManager gameManager) {
            gameManager.TransitionTo(new PlayingState());
        }
    }
}