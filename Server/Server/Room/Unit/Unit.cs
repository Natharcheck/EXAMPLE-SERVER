using System.Numerics;
using Server.Physics;
using Server.Physics.Hood;
using Server.Physics.Hood.Components;
using Server.Room.Componets;
using Server.Room.Interface;

namespace Server.Room;

public abstract class Unit : AtomicObject, IHealth, IAbilityAttack
{
    public string Team = "Defualt";
    
    public EUBehavior Behavior;
    public ETypeUnit TypeUnit;
    public ushort Index { get; set; }

    private Transform Target;
    
    public UHealth? HealthComponent { get; set; }
    public AbilityStorage AbilityStorage { get; set; }
    public AbilityCastHandler AbilityCastHandler { get; set; }

    private List<UBooster> Boosters { get; set; }

    public ushort Character { get; set; }
    public ushort ConnectionId { get; set; }
    public bool IsReady { get; set; }

    public ushort AngleAttack { get; set; }
    public float MoveSpeed { get; set; }
    public float MaxMoveSpeed { get; set; }

    public short KnockdownTime { get; set; }
    
    public bool IsAttack { get; set; }
    public bool IsMove { get; set; }

    public short BoostValue { get; set; }

    public delegate void WhenUnitKnockdown(ushort connectionId);
    public delegate void WhenUnitDead(ushort connectionId);

    public event WhenUnitKnockdown? UnitKnockdown;
    public event WhenUnitDead? UnitDead;
    
    protected virtual void Initialize(AbilityConfig[] abilityConfigs, UHealth healthComponent)
    {
        Collision.Radius = 1;
        KnockdownTime = 5;
        
        HealthComponent = healthComponent;
        HealthComponent.MinimumHealth += OnDead; 

        Boosters = new();

        if (abilityConfigs != null)
        {
            AbilityStorage = new AbilityStorage(abilityConfigs);
            AbilityCastHandler = new AbilityCastHandler(this, AbilityStorage);
            AbilityCastHandler.Initialize();   
        }
    }

    public virtual void UseAbility(byte abilityIndex, byte abilityState = 1)
    {
        if (AbilityCastHandler.CheckStateCast(abilityIndex, abilityState))
        {
            AbilityCastHandler.OnCast(abilityIndex);   
        }
    }

    public virtual void TackDamage(short damage)
    {
        HealthComponent!.Health -= damage;
    }

    public virtual void TackHealing(short healing)
    {
        HealthComponent!.Health += healing;
    }

    public async void Knockdown()
    {
        while (KnockdownTime > 0)
        {
            KnockdownTime--;
            
            if (KnockdownTime <= 1)
            {
                OnDead();
            }

            await Task.Delay(1000);
        }
    }

    public virtual void OnDead()
    {
        UnitDead?.Invoke(Index);
        OnDisable();
    }
    
    public virtual void OnUnitKnockdown(ushort connectionId)
    {
        UnitKnockdown?.Invoke(connectionId);
    }

    public virtual void AddBooster(UBooster booster)
    {
        Boosters.Add(booster);
        
        HealthComponent!.MaxHealth += 100;
        BoostValue += 100;
        
        booster.SetActive(false);
    }

    public virtual void SetTarget(AtomicObject atomicObject)
    {
        Target = atomicObject.Transform;
    }

    public virtual Vector2 GetTargetPosition() => Target.Position;

    public virtual List<UBooster> GetBoosters()
    {
        foreach (var booster in Boosters)
        {
            booster.SetActive(true);
        }

        HealthComponent!.MaxHealth -= BoostValue;
        
        return Boosters;
    }
}