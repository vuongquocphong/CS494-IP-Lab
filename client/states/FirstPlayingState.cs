namespace StateManager {
    public class FirstPlayingState : IState {
        public void Handle(GameComponents.GameManager gameManager) {
            gameManager.TransitionTo(new PlayingState());
        }
    }
}