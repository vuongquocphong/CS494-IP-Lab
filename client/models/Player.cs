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
        public char GuessChar()
        {
            // Ask user to input character
            return 'c';
        }
        public string GuessWord()
        {
            // Ask user to input word
            return "word";
        }
    }
}