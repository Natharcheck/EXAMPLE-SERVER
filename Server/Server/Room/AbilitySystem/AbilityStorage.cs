namespace Server.Room;

public class AbilityStorage(AbilityConfig[] abilityConfigs)
{
    private readonly AbilityConfig[] _configs = abilityConfigs;
    private readonly List<Ability> _abilities = new();

    public void Initialize()
    {
        foreach (var config in _configs)
        { 
            var builder = config.GetBuilder();
                builder.Build();
                
            _abilities.Add(builder.GetResult());
        }
    }

    public Ability[] GetAbilities() => _abilities.ToArray();
    public Ability GetAbility<T>() where T : Ability
    {
        foreach (var ability in _abilities)
        {
            if (ability is T)
            {
                return ability;
            }
        }

        return null!;
    }
}