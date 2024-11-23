namespace Server.Room.AbilitySystem.Example;

public class HealthBuilder(HealthConfig config) : AbilityBuilder(config)
{
    private readonly HealthConfig _config = config;

    public override void Build()
    {
        _ability = new HealthAbility(_config.Healing);
        
        base.Build();
    }
}