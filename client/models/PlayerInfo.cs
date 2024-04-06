namespace GameComponents
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
    class PlayerInfo(string name)
    {

        public string Name { get; set; } = name;
        public int Point { get; set; } = 0;
        public bool ReadyStatus { get; set; } = false;
        public PlayerState State { get; set; } = PlayerState.Playing;
    }
}