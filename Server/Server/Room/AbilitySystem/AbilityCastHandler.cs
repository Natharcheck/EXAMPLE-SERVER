using Microsoft.Extensions.DependencyInjection;
using Server.Physics;
using Server.Room.AbilitySystem;

namespace Server.Room;

public class AbilityCastHandler(Unit owner, AbilityStorage abilityStorage)
{
    private readonly Unit _owner = owner;
    private readonly AbilityStorage _storage = abilityStorage;

    private List<Unit> _units;
    private List<Ability> _abilities = new();
    private Ability _currentAbility;

    private readonly float _deltaTime = ServiceContainer.Provider.GetRequiredService<Options>().DeltaTime;
    private readonly ushort _updateTime = ServiceContainer.Provider.GetRequiredService<Options>().UpdateTime;
    private ushort _localUpdateTime = 0;

    public void Initialize()
    {
        _localUpdateTime = (ushort)(_updateTime / 1000);
        _storage.Initialize();
        _abilities.AddRange(_storage.GetAbilities());
        
        Update();
    }

    public void OnCast(byte abilityIndex)
    {
        switch (_abilities[abilityIndex].AbilityStatus)
        {
            case EAbilityStatus.Ready:
                _currentAbility = _abilities[abilityIndex];
                _currentAbility.StartCast(_units, _owner);
                break;
            case EAbilityStatus.Cooldown:
                break;
            case EAbilityStatus.NeedMana:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public bool CheckStateCast(byte abilityIndex, byte abilityState)
    {
        _currentAbility = _abilities[abilityIndex];
        _currentAbility.AbilityState = (EAbilityState)abilityState;

        switch (_currentAbility.AbilityState)
        {
            case EAbilityState.Cast: return true;
            case EAbilityState.Cancel:
                _currentAbility.CancelCast();
                return false;
            default:
                return false;
                    
        }
    } 

    public void SetUnits(List<Unit> units) => _units = units;

    public void SetUnits(List<Player> players)
    {
        _units = new List<Unit>();
        _units.AddRange(players);
    }

    private async void Update()
    {
        while (true)
        {
            foreach (var ability in _abilities)
                ability.EvenTick(_localUpdateTime);
            
            await Task.Delay(_updateTime);
        }
    }

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