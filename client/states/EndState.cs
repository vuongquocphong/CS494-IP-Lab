using GameComponents;

namespace StateManager {
    public class EndState : State {
        private ScoreboardPanel _scoreboardPanel = new();
        override
        public void Handle(Event ev) {
            switch (ev) {
                case Event.QUIT:
                    Console.WriteLine("Game Over");
                    _gameManager.TransitionTo(new StartState());
                    break;
                case Event.RESTART:
                    Console.WriteLine("You Win");
                    _gameManager.TransitionTo(new WaitingState());
                    break;
                default:
                    throw new System.NotImplementedException();
            }
        }
        override
        public void OnReady() {
            List<(string, int)> players = [];
            foreach (var player in _gameManager.PlayersList) {
                players.Add((player.Name, player.Point));
            }
            players.Sort((a, b) => b.Item2.CompareTo(a.Item2));
            _scoreboardPanel.SetPlayers(players);
        }
    }
}