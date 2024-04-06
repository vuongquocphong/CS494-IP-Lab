namespace StateManager {
    public class StartState : IState {
        public void Handle(GameComponents.GameManager gameManager) {
            gameManager.TransitionTo(new PlayingState());
        }
    }
}