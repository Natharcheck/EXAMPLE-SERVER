using System.Numerics;
using Server.Physics.Hood;
using Server.Room.AbilitySystem;

namespace Server.Room;

public abstract class Ability : AtomicObject
{
    public EAbilityStatus AbilityStatus;
    public EAbilityState AbilityState;
    
    public ushort CooldownTimer;
    public ushort ManaCost;
    public ushort CooldownTime;

    public void SetCooldownTime(ushort cooldownTime) => CooldownTimer = cooldownTime;
    public void SetManaCost(ushort manaCost) => ManaCost = manaCost;
    public void SetAbilityStatus(EAbilityStatus status) => AbilityStatus = status;
    public void ChangeCooldownTimer(ushort timer)
    {
        CooldownTimer = (ushort)Math.Clamp(timer, 0f, CooldownTime);
    }

    public virtual void StartCast(List<Unit> units, Unit owner)
    {
        
    }

    public virtual bool CheckCondition(Unit? owner, Unit? target, Vector2 location = default)
    {
        return false;
    }

    public virtual void ApplyCast()
    {
        
    }

    public virtual void EvenTick(ushort deltaTick)
    {
        
    }

    public virtual void CancelCast()
    {
        
    }
}