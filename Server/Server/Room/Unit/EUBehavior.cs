namespace Server.Room;

public enum EUBehavior : ushort
{
    Idle = 1,
    Move, 
    Attack,
    Ability,
    Knockdown,
    Dead
}