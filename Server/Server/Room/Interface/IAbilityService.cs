using System.Numerics;

namespace Server.Room.Interface;

public interface IAbilityService
{
    public void UseAbility(ushort connectionId, byte abilityIndex, byte abilityState, ushort angle, Vector2 targetPositionAbility);
}