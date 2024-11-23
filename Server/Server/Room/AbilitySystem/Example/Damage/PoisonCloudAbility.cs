using System.Numerics;

namespace Server.Room.AbilitySystem.Example.Damage;

public class PoisonCloudAbility(short damage, short range, float speed) : Ability
{
    private short Damage { get; set; } = damage;
    private short Range { get; set; } = range;
    private float Speed { get; set; } = speed;
    
    private Unit? _owner;
    private Unit? _target;
    
    public override void StartCast(List<Unit> units, Unit owner)
    {
        Collision.Radius -= Speed;

        if (Collision.Radius <= 0)
        {
            Collision.Radius = 0;
        }
        
        foreach (var unit in units)
        {
            if (Physics.Physics.CheckCollisionCirclesInCircle(unit, this))
            {
                if(CheckCondition(unit, owner))
                {
                    ApplyCast();   
                }
            }
        }
    }

    public override bool CheckCondition(Unit? target, Unit? owner, Vector2 location = default)
    {
        _target = target;
        
        return true;
    }

    public override void ApplyCast()
    {
        if (_target != null)
        {
            _target.TackDamage(Damage);
            
            SetAbilityStatus(EAbilityStatus.Ready);
        }
    }

    public override void EvenTick(ushort deltaTick)
    {
        
    }
}