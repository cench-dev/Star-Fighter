using StarFighter.Model;

namespace StarFighter.View
{
    internal class GameView(GameModel starFighterModel)
    {
        private readonly GameModel gameModel = starFighterModel;
        private static readonly Image playerSprite = Image.FromFile("source/X-WING_SPRITE.png");
        private static readonly Image enemySprite = Image.FromFile("source/TIE_SPRITE.png");
        private static readonly Image backgroundSprite = Image.FromFile("source/Star_Wars_background_night_sky_white_stars.jpeg");
        private static readonly Image pauseIcon = Image.FromFile("source/pause_icon.png");
        private readonly Font usingBaseFont = new Font("Impact", 16);
        private readonly Font usingBoldFont = new Font("Impact", 32);
        private readonly Brush baseColor = Brushes.Yellow;

        public void Paint(PaintEventArgs e)
        {
            e.Graphics.DrawImage(backgroundSprite, 0, 0, e.ClipRectangle.Width, e.ClipRectangle.Height);

            switch (gameModel.state.CurrentScreen)
            {
                case GameScreen.MusicWarning:
                    DrawMusicWarning(e.Graphics, e.ClipRectangle);
                    break;
                case GameScreen.MainMenu:
                    DrawMainMenu(e.Graphics, e.ClipRectangle);
                    break;
                case GameScreen.GameRules:
                    DrawGameRules(e.Graphics, e.ClipRectangle);
                    break;
                case GameScreen.GamePlay:
                    DrawGamePlay(e.Graphics);
                    break;
                case GameScreen.MenuPause:
                    DrawPauseMenu(e.Graphics);
                    break;
            }
        }

        private void DrawMusicWarning(Graphics e, Rectangle bounds)
        {
            DrawCenteredString(e, "ATTENTION", usingBoldFont, baseColor, bounds.Width, 100);

            var warningText = "В игре присутствует фоновая музыка\nЕсли вы не хотите её слушать убавьте звук на вашем устройстве.\nМузыку можно отключить в меню игры";
            DrawCenteredString(e, warningText, usingBaseFont, baseColor, bounds.Width, 200);
            DrawCenteredString(e, "Click To Continue", usingBaseFont, baseColor, bounds.Width, 400);
        }

        private void DrawMainMenu(Graphics e, Rectangle bounds)
        {
            DrawCenteredString(e, "STAR FIGHTER", usingBoldFont, baseColor, bounds.Width, bounds.Height / 3);
            DrawCenteredString(e, "CLICK TO START", usingBaseFont, baseColor, bounds.Width, bounds.Height / 2);
        }

        private void DrawGameRules(Graphics e, Rectangle bounds)
        {
            var startY = 180;
            DrawCenteredString(e, "HOW TO PLAY", usingBoldFont, baseColor, bounds.Width, 100);

            e.DrawString($"Move Left: {gameModel.setting.MoveLeftKey}", 
                usingBaseFont, 
                baseColor, 
                bounds.Width / 2 - 100, startY);

            e.DrawString($"Move Right: {gameModel.setting.MoveRightKey}",
                usingBaseFont, 
                baseColor, 
                bounds.Width / 2 - 100, 
                startY + 40);

            e.DrawString($"Shoot: Left Mouse Button", 
                usingBaseFont, 
                baseColor, 
                bounds.Width / 2 - 100, startY + 80);

            e.DrawString($"Menu: Escape",
                usingBaseFont,
                baseColor,
                bounds.Width / 2 - 100, startY + 120);

            DrawCenteredString(e, "Click to continue", usingBaseFont, baseColor, bounds.Width, bounds.Height - 100);
        }

        private void DrawGamePlay(Graphics e) 
        {
            e.DrawImage(backgroundSprite, 0, gameModel.BackgroundOffsetY, GameModel.CurrentWidth, GameModel.CurrentHeight);

            e.DrawImage(backgroundSprite, 0, gameModel.BackgroundOffsetY - GameModel.CurrentHeight, GameModel.CurrentWidth, GameModel.CurrentHeight);
            foreach (var bullet in gameModel.player.Bullets)
            {
                var bulletRect = new Rectangle(
                    bullet.X,
                    bullet.Y,
                    5, 10);
                e.FillRectangle(Brushes.Lime, bulletRect);
            }

            foreach (var bullet in gameModel.enemyBullets)
            {
                var bulletRect = new Rectangle(
                    bullet.X,
                    bullet.Y,
                    5, 10);
                e.FillRectangle(Brushes.Red, bulletRect);
            }

            foreach (var enemy in gameModel.enemies)
                e.DrawImage(enemySprite, enemy.X, enemy.Y, 50, 50);

            e.DrawImage(playerSprite, gameModel.player.X, gameModel.player.Y, 60, 60);

            e.DrawString(
                $"POINTS:\n {gameModel.state.Score}",
                usingBaseFont,
                baseColor,
                680, 10);

            e.DrawString(
                $"HEALTH: {gameModel.player.Health}",
                usingBaseFont,
                baseColor,
                680, 500);

            e.DrawString(
                 $"RECORD:\n {gameModel.HighScore}",
                 usingBaseFont,
                 baseColor,
                 680, 60);

            if (gameModel.state.IsGameOver)
            {
                e.DrawString($"GAME OVER\nYOUR POINTS: {gameModel.state.Score}\nPRESS R TO RESTART THE GAME",
                    usingBoldFont,
                    baseColor,
                    150, 150);
            }

            var pauseButtonRect = new Rectangle(10, 10, 30, 30);
            e.FillRectangle(Brushes.Transparent, pauseButtonRect);

            e.DrawImage(pauseIcon, pauseButtonRect.X + 5, pauseButtonRect.Y + 5);
        }

        private void DrawPauseMenu(Graphics e)
        {
            DrawCenteredString(e, "PAUSE", usingBoldFont, baseColor, 800, 150);
            DrawMenuItem(e, "Продолжить", 250);
            DrawMenuItem(e, gameModel.MusicEnabled ? "Выключить музыку" : "Включить музыку", 320);
            DrawMenuItem(e, "Выйти из игры", 390);
            
        }

        private void DrawMenuItem(Graphics e, string text, int ordinate)
        {
            var centerAbcissa = 400;
            var textSize = e.MeasureString(text, usingBaseFont);
            var abcissa = centerAbcissa - textSize.Width / 2;
            e.DrawString(text, usingBaseFont, Brushes.Yellow, abcissa, ordinate);
        }

        private void DrawCenteredString(Graphics e, string text, Font font, Brush brush, int totalWidth, int y)
        {
            var textSize = e.MeasureString(text, font);
            var x = (totalWidth - textSize.Width) / 2;
            e.DrawString(text, font, brush, x, y);
        }
    }
}
