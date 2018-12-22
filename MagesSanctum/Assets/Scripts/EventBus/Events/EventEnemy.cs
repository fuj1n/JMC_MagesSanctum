public static class EventEnemy
{
    public struct Spawned : IEventBase { }

    public struct Died : IEventBase
    {
        public int coinReward;

        public Died(int coinReward)
        {
            this.coinReward = coinReward;
        }
    }

    public struct Passed : IEventBase
    {
        public float damage;

        public Passed(float damage)
        {
            this.damage = damage;
        }
    }

    public struct SpawnClock : IEventBase
    {
        public Enemy template;

        public SpawnClock(Enemy template)
        {
            this.template = template;
        }
    }
}
