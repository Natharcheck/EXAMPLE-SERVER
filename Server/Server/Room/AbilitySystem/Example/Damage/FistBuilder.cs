namespace Server.Room.AbilitySystem.Example.Damage;

public class FistBuilder(FistConfig config) : AbilityBuilder(config)
{
    private readonly FistConfig _config = config;

    public override void Build()
    {
        _ability = new FistAbility(_config.Damage, _config.Range);
        
        base.Build();
    }
}