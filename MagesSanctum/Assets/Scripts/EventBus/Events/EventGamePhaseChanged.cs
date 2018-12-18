public struct EventGamePhaseChanged : IEventBase
{
    public GamePhase phase;

    public EventGamePhaseChanged(GamePhase phase)
    {
        this.phase = phase;
    }
}
