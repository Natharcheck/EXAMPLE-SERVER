namespace Server.Room;

public class AbilityConfig
{
    public string? Title { get; protected set; }
    public ushort CooldownTimer { get; protected set; }
    public ushort CooldownTime { get; protected set; }
    public ushort ManaCost { get; protected set; }

    public virtual AbilityBuilder GetBuilder() => new AbilityBuilder(this);
}