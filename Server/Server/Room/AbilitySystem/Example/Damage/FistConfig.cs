namespace Server.Room.AbilitySystem.Example.Damage;

public class FistConfig : AbilityConfig
{
    public readonly short Damage = 100;
    public readonly short Range = 1;

    private void Initialize()
    {
        CooldownTime = 0;
        CooldownTimer = 0;
        ManaCost = 0;
    }
    
    public override AbilityBuilder GetBuilder()
    {
        Initialize();
        
        return new FistBuilder(this);
    }
}