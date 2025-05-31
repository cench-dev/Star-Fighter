using StarFighter.Model;

namespace StarFighter.Contoller
{
    internal class GameController
    {
        private readonly GameModel gameModel;
        private readonly System.Windows.Forms.Timer spawnTimer;
        private readonly Random random = new Random();

        public GameController(GameModel model)
        {
            gameModel = model;
            spawnTimer = new System.Windows.Forms.Timer { Interval = 2000 };
            spawnTimer.Tick += (s, e) =>
            {
                if (gameModel.state.CurrentScreen == GameScreen.GamePlay)
                    model.SpawnEnemy(GameModel.CurrentWidth, random);
            };
            spawnTimer.Start();
        }

        public void MoveInput(Keys key, int screenWidth)
        {
            if (key == gameModel.setting.MoveLeftKey)
                gameModel.player.MoveLeft(0);
            else if (key == gameModel.setting.MoveRightKey)
                gameModel.player.MoveRight(screenWidth);
        }

        public void Update()
        {
            if (gameModel.state.IsGameOver)
                return;

            gameModel.BackgroundOffsetY += GameModel.BackgroundSpeed;
            if (gameModel.BackgroundOffsetY >= GameModel.CurrentHeight)
                gameModel.BackgroundOffsetY = 0;

            var playerRect = new Rectangle(
                    gameModel.player.X,
                    gameModel.player.Y,
                    50, 50);

            foreach (var bullet in gameModel.player.Bullets.ToList())
            {
                bullet.Update();
                if (bullet.Y < 0)
                    gameModel.player.Bullets.Remove(bullet);
            }

            foreach (var bullet in gameModel.enemyBullets.ToList())
                bullet.Update();

            foreach (var enemy in gameModel.enemies.ToList())
            {
                enemy.Update();
                var enemyRect = new Rectangle(enemy.X, enemy.Y, 50, 50);
                if (playerRect.IntersectsWith(enemyRect))
                {
                    gameModel.enemies.Remove(enemy);
                    gameModel.player.Health -= 25;

                    if(!gameModel.player.IsAlive)
                    {
                        GameOver();
                        return;
                    }
                    continue;
                }

                if (enemy.CanShoot && random.Next(0, 100) < 3)
                {
                    gameModel.enemyBullets.Add(new Bullet
                    (
                        enemy.X + 25,
                        enemy.Y + 50,
                        isPlayerBullet: false
                    ));
                    enemy.TimerBetweenShoot = Enemy.ShootInterval;
                }

            }

            if (!gameModel.state.IsGameOver && random.Next(0, 100) < 2)
                gameModel.SpawnEnemy(GameModel.CurrentWidth, random);

            foreach (var bullet in gameModel.player.Bullets.ToList())
            {
                foreach (var enemy in gameModel.enemies.ToList())
                    if (bullet.X < enemy.X + 50 && bullet.X + 5 > enemy.X && bullet.Y < enemy.Y + 50 && bullet.Y + 10 > enemy.Y)
                    {
                        gameModel.player.Bullets.Remove(bullet);
                        gameModel.enemies.Remove(enemy);
                        gameModel.state.Score += 100;
                        break;
                    }
            }

            foreach(var bullet in gameModel.enemyBullets.ToList())
            {
                var bulletRect = new Rectangle(bullet.X, bullet.Y, 5, 15);
                if (playerRect.IntersectsWith(bulletRect))
                {
                    gameModel.enemyBullets.Remove(bullet);
                    gameModel.player.Health -= 10;
                    if (!gameModel.player.IsAlive)
                        GameOver();
                }
            }    
        }

        public void Shoot() => gameModel.player.Shoot();

        private void GameOver()
        {
            gameModel.state.IsGameOver = true;
            gameModel.CheckHighScore();
            spawnTimer.Stop();
            
        }

        public void Restart()
        {
            gameModel.player.Health = 100;
            gameModel.state.IsGameOver = false;
            spawnTimer.Start();
        }
    }
}
