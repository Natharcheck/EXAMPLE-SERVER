using Server.Physics.Hood;

namespace Server.Physics;

public abstract class Block(byte type) : AtomicObject
{
    public byte Type = type;
}