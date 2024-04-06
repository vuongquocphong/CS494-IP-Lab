namespace StateManager {
    public class ScoreboardState : IState {
        public void Handle(GameComponents.GameManager gameManager) {
            gameManager.TransitionTo(new PlayingState());
        }
    }
}