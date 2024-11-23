namespace Server.Room.Interface;

public interface IAbilityAttack
{
    public AbilityStorage AbilityStorage { get; set; }
    public AbilityCastHandler AbilityCastHandler { get; set; }
    public void UseAbility(byte abilityIndex, byte abilityState);
}