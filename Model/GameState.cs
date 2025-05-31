namespace StarFighter.Model
{
    public enum GameScreen
    {
        MusicWarning,
        MainMenu,
        GameRules,
        GamePlay,
        MenuPause
    }

    internal class GameState
    {
        public bool IsPaused { get; set; }
        public int Score { get; set; }
        public bool IsGameOver { get; set; }
        public GameScreen CurrentScreen { get; set; } = GameScreen.MusicWarning;

        public void IncreaseScore(int points) => Score += points;
    }
}
