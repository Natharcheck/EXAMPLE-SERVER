namespace Server.Room.AbilitySystem;

public enum EAbilityStatus : byte
{
    None = 1, 
    Ready, 
    Cooldown,
    NeedMana
}