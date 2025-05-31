using StarFighter.Contoller;
using StarFighter.Model;
using StarFighter.View;

namespace StarFighter
{
    public partial class MainForm : Form
    {
        private readonly GameModel gameModel;
        private readonly GameController gameController;
        private readonly GameView gameView;
        private readonly System.Windows.Forms.Timer gameTimer;

        public MainForm()
        {
            InitializeComponent();
            DoubleBuffered = true;
            KeyPreview = true;
            gameModel = new GameModel();
            gameModel.LoadHighScore();
            gameController = new GameController(gameModel);
            gameView = new GameView(gameModel);
            gameTimer = new System.Windows.Forms.Timer { Interval = 16 };
            gameTimer.Tick += (s, e) =>
            {
                if (gameModel.state.CurrentScreen == GameScreen.GamePlay)  
                    gameController.Update();
                Invalidate();
            };
            gameTimer.Start();
        }

        private static bool IsMouseOverText(Point mousePos, string text, int y)
        {
            var textSize = TextRenderer.MeasureText(text, new Font("Impact", 16));
            var centerX = 400;
            var x = centerX - textSize.Width / 2;

            return mousePos.X >= x && mousePos.X <= x + textSize.Width && mousePos.Y >= y && mousePos.Y <= y + textSize.Height;
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            if (gameModel.state.CurrentScreen == GameScreen.MenuPause)
            {
                if (IsMouseOverText(e.Location, "Продолжить", 250))
                {
                    gameModel.state.IsPaused = false;
                    gameModel.state.CurrentScreen = GameScreen.GamePlay;
                }
                else if (IsMouseOverText(e.Location, "Выключить музыку", 320) || IsMouseOverText(e.Location, "Включить музыку", 320))
                {
                    gameModel.MusicEnabled = !gameModel.MusicEnabled;
                    if (gameModel.MusicEnabled)
                        MusicPlayer.Play();
                    else 
                        MusicPlayer.Pause();
                }
                else if (IsMouseOverText(e.Location, "Выйти из игры", 390))
                    Application.Exit();

                Invalidate();
            }

            if (gameModel.state.CurrentScreen == GameScreen.GamePlay && !gameModel.state.IsGameOver)
            {
                var pauseButtonRect = new Rectangle(10, 10, 30, 30);
                if (pauseButtonRect.Contains(e.Location))
                {
                    gameModel.state.IsPaused = true;
                    gameModel.state.CurrentScreen = GameScreen.MenuPause;
                    Invalidate();
                    return;
                }
            }

            switch (gameModel.state.CurrentScreen)
            {
                case GameScreen.MusicWarning:
                    gameModel.state.CurrentScreen = GameScreen.MainMenu;
                    MusicPlayer.Play();
                    break;
                case GameScreen.MainMenu:
                    gameModel.state.CurrentScreen = GameScreen.GameRules;
                    break;

                case GameScreen.GameRules:
                    gameModel.state.CurrentScreen = GameScreen.GamePlay;
                    break;
                case GameScreen.GamePlay:
                    if (!gameModel.state.IsGameOver)
                        gameController.Shoot();
                    break;
            }
            Invalidate();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                if (gameModel.state.CurrentScreen == GameScreen.GamePlay)
                {
                    gameModel.state.IsPaused = true;
                    gameModel.state.CurrentScreen = GameScreen.MenuPause;
                }

                else if (gameModel.state.CurrentScreen == GameScreen.MenuPause)
                {
                    gameModel.state.IsPaused = false;
                    gameModel.state.CurrentScreen = GameScreen.GamePlay;
                }
                Invalidate();
            }


            if (gameModel.state.CurrentScreen != GameScreen.GamePlay)
                return;

            gameController.MoveInput(e.KeyCode, ClientSize.Width);

            if (e.KeyCode == Keys.R && gameModel.state.IsGameOver)
            {
                gameModel.Reset();
                gameController.Restart();
                Invalidate();
            }
        }
        

        protected override void OnPaint(PaintEventArgs e) => gameView.Paint(e);

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            gameModel.CheckHighScore();
            gameModel.SaveHighScore();
            base.OnFormClosing(e);
        }
    }
}
