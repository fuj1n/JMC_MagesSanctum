public struct EventEnemySpawnClock : IEventBase
{
    public Enemy template;

    public EventEnemySpawnClock(Enemy template)
    {
        this.template = template;
    }
}
