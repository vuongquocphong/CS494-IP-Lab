namespace StateManager {
    public class EndState : IState {
        public void Handle(GameComponents.GameManager gameManager) {
            gameManager.TransitionTo(new PlayingState());
        }
    }
}