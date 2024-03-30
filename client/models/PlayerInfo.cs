namespace PlayerInfo
{
    enum PlayerState
    {
        Playing,
        Lost,
        Disconnected, 
        Win
    }

    enum GuessType
    {
        Character,
        Word
    }
    class PlayerInfo
    {

        public string Name { get; set; }
        public int Point { get; set; }
        public bool ReadyStatus { get; set; }
        public PlayerState State { get; set; }
        public GuessType GuessType { get; set; }
        public string Guess { get; set; }

        public PlayerInfo()
        {
            Name = "";
            Point = 0;
            ReadyStatus = false;
            State = PlayerState.Playing;
            GuessType = GuessType.Character;
            Guess = "";
        }
        public PlayerInfo(string name)
        {
            Name = name;
            Point = 0;
            ReadyStatus = false;
            State = PlayerState.Playing;
            GuessType = GuessType.Character;
            Guess = "";
        }
    }
}