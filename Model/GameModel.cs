namespace StarFighter.Model
{
    internal class GameModel
    {
        public Player player = new Player { X = 400, Y = 500 };
        public List<Enemy> enemies = new List<Enemy>();
        public List<Bullet> bullets = new List<Bullet>();
        public GameState state = new GameState();
        public GameSetting setting = new GameSetting();
        public List<Bullet> enemyBullets = new List<Bullet>();
        public int BackgroundOffsetY { get; set; } = 0;
        public const int BackgroundSpeed = 40;
        public static int CurrentWidth = 800;
        public  static int CurrentHeight = 600;
        public bool MusicEnabled {  get; set; } = true;
        private const string HighScoreFile = "highscore.txt";
        public int HighScore { get; set; }

        public void LoadHighScore()
        {
            var scoreText = File.ReadAllText(HighScoreFile);
            HighScore = int.Parse(scoreText);

        }
        public void SaveHighScore() => File.WriteAllText(HighScoreFile, HighScore.ToString());

        public void CheckHighScore()
        {
            if (state.Score > HighScore)
            {
                HighScore = state.Score;
                SaveHighScore();
            }
        }

        public void SpawnEnemy(int screenWidth, Random random)
        {
            var enemy = new Enemy
            {
                X = random.Next(0, screenWidth - 50),
                Y = -50,
            };
            enemies.Add(enemy);
        }

        public void Reset()
        {
            enemies.Clear();
            bullets.Clear();
            state.Score = 0;
            state.IsGameOver = false;
        }
    }
}
