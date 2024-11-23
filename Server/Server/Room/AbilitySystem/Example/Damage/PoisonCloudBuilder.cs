using System.Numerics;

namespace Server.Room.AbilitySystem.Example.Damage;

public class PoisonCloudBuilder(PoisonCloudConfig config) : AbilityBuilder(config)
{
    private readonly PoisonCloudConfig _config = config;

    public override void Build()
    {
        _ability = new PoisonCloudAbility(_config.Damage, _config.Range, _config.Speed);
        _ability.Transform.Position = new Vector2(45, 45);
        _ability.Collision.Radius = _config.Range;
        
        base.Build();
    }
}