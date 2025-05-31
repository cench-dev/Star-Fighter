namespace StarFighter.Model
{
    internal class Player
    {
        public int X;
        public int Y;
        public int Health = 100;
        public const int Speed = 50;
        public List<Bullet> Bullets = new List<Bullet>();
        public bool IsAlive => Health > 0;

        public void MoveLeft(int screenLeftBorder)
        {
            if (X - Speed >= screenLeftBorder) 
                X -= Speed;
        }
        public void MoveRight(int screenRightBorder)
        {
            if (X  + Speed <= screenRightBorder)
                X += Speed;
        }

        public void Shoot() => Bullets.Add(new Bullet(X + 25, Y - 20, isPlayerBullet: true));
    }
}
