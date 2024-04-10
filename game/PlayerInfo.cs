namespace GameComponents
{
    public enum PlayerState
    {
        Playing,
        Lost,
        Disconnected, 
        Win
    }

    public enum GuessType
    {
        Character,
        Word
    }
    
    public class PlayerInfo(string name)
    {
        public string Name { get; set; } = name;
        public int Point { get; set; } = 0;
        public bool ReadyStatus { get; set; } = false;
        public PlayerState State { get; set; } = PlayerState.Playing;
    }
}