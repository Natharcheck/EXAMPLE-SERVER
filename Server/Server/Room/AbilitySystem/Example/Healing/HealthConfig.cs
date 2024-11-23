namespace Server.Room.AbilitySystem.Example;

public class HealthConfig : AbilityConfig
{
    public readonly short Healing = 100;

    private void Initialize()
    {
        CooldownTime = 0;
        CooldownTimer = 0;
        ManaCost = 0;
    }
    
    public override AbilityBuilder GetBuilder()
    {
        Initialize();
        return new HealthBuilder(this);
    }
}