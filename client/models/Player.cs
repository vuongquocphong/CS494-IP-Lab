namespace Player
{
    class Player: PlayerInfo.PlayerInfo
    {
        public Player(): base()
        {
        }
        public Player(string name): base(name)
        {
        }
        public void SendGuess()
        {
            // Creation of message that
            // we will send to Server
        }
    }
}