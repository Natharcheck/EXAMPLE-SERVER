namespace Server.Room.AbilitySystem.Example.Damage;

public class PoisonCloudConfig : AbilityConfig
{
    public short Damage = 10;
    public short Range = 140;
    public float Speed = 1;

    private void Initialize()
    {
        CooldownTime = 0;
        CooldownTimer = 0;
        ManaCost = 0;
    }
    
    public override AbilityBuilder GetBuilder()
    {
        Initialize();
        
        return new PoisonCloudBuilder(this);
    }
}