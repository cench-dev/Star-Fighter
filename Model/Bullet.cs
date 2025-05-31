namespace StarFighter.Model
{
    internal class Bullet
    {
        public int X { get; set; }
        public int Y { get; set; }
        public const int Speed = 25;
        public bool IsPlayerBullet { get; }

        public Bullet(int x, int y, bool isPlayerBullet)
        {
            X = x;
            Y = y;
            IsPlayerBullet = isPlayerBullet;
        }

        public void Update()
        {
            if (IsPlayerBullet)
                Y -= Speed;
            else
                Y += Speed;
        }
    }
}
