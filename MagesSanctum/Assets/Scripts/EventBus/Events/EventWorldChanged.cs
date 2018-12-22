public struct EventWorldChanged : IEventBase
{
    public HexBuilder parent;

    public EventWorldChanged(HexBuilder parent)
    {
        this.parent = parent;
    }
}
