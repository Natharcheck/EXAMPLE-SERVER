using Server.Room.AbilitySystem;

namespace Server.Room;

public class AbilityBuilder(AbilityConfig config)
{
    private readonly AbilityConfig _config = config;
    protected Ability _ability = null!;

    public virtual void Build()
    {
        if (_ability != null)
        {
            _ability.SetManaCost(_config.ManaCost);
            _ability.SetCooldownTime(_config.CooldownTime);
            _ability.SetAbilityStatus(EAbilityStatus.Ready);
        }
    }

    public Ability GetResult() => _ability;
}