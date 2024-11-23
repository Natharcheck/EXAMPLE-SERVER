using Microsoft.Extensions.DependencyInjection;
using Server.Physics;
using Server.Room.AbilitySystem.Example.Damage;
using Server.Room.Componets;

namespace Server.Room.Example;

public class SpongeBob : Player
{
    private readonly float _deltaTime = ServiceContainer.Provider.GetRequiredService<Options>().DeltaTime;
    public SpongeBob()
    {
        UHealth health = new UHealth(600, 600, 0);
        
        var abilityConfigs = new AbilityConfig[1];
            abilityConfigs[0] = new FistConfig(); 
            
        MoveSpeed = 5 * _deltaTime;
        MaxMoveSpeed = 25 * _deltaTime;
        
        Initialize(abilityConfigs, health);
    }

}