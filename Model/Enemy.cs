namespace StarFighter.Model
{
    public class Enemy
    {
        public int X { get; set; }
        public int Y { get; set; }
        public const int Speed = 6;
        public int Health { get; set; } = 100;
        public int TimerBetweenShoot { get; set; } = 0;
        public const int ShootInterval = 100;
        public void Update()
        {
            Y += Speed;
            if (TimerBetweenShoot > 0)
                TimerBetweenShoot--;
        }

        public bool CanShoot => TimerBetweenShoot <= 0;

        public void TakeDamage(int damage)
        {
            if (damage > 0)
                Health -= damage;
        }

    }
}
