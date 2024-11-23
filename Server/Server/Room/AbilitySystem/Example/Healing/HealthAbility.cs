using System.Numerics;
using Server.Physics;

namespace Server.Room.AbilitySystem.Example;

public class HealthAbility(short healing) : Ability
{
    private short Healing { get; set; } = healing;
    
    private Unit? _owner;
    private Unit? _target;

    public override void StartCast(List<Unit> units, Unit owner)
    {
        _owner = owner;
        
        foreach (var unit in units)
        {
            if (Physics.Physics.CheckCollisionCircles(unit, _owner))
            {
                if(CheckCondition(unit, _owner))
                {
                    ApplyCast();
                }
            }
        }
    }

    public override bool CheckCondition(Unit? target, Unit? owner, Vector2 location = default)
    {
        if (owner == null || target == null) return false;

        if (owner.Team == target.Team)
        {
            _target = target;
            return true;
        }
        
        return false;
    }

    public override void ApplyCast()
    {
        if (_target != null)
        {
            _target.TackHealing(Healing);
            SetAbilityStatus(EAbilityStatus.Cooldown);
        }
    }

    public override void EvenTick(ushort deltaTick)
    {
        if (AbilityStatus == EAbilityStatus.Cooldown)
        {
            ChangeCooldownTimer(deltaTick);

            if (CooldownTimer <= 0)
            {
                SetAbilityStatus(EAbilityStatus.Ready);
            }
        }
    }
}