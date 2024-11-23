using System.Numerics;
using Server.Physics.Hood;

namespace Server.Room.AbilitySystem.Example.Damage;

public class FistAbility(short damage, short range) : Ability
{
    private short Damage { get; set; } = damage;
    private short Range { get; set; } = range;
    
    private Unit? _owner;
    private Unit? _target;

    public override void StartCast(List<Unit> units, Unit owner)
    {
        _owner = owner;
        foreach (var unit in units)
        {
            if (Physics.Physics.CheckCollisionInFrontHalf(unit, _owner))
            {
                if (Physics.Physics.CheckCollisionCircles(unit, _owner, range))
                {
                    if(CheckCondition(unit, _owner))
                    {
                        ApplyCast();
                    }
                }
            }
        }
    }

    public override bool CheckCondition(Unit? target, Unit? owner, Vector2 location = default)
    {
        if (owner == null || target == null) return false;
        if (target == owner) return false;
        
        if (owner.Team != target.Team)
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
            _owner?.Transform.LookAt(_owner.AngleAttack);
            _target.TackDamage(Damage);
            
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