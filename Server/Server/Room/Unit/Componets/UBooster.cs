using Server.Physics.Hood;

namespace Server.Room.Componets;

public class UBooster : AtomicObject
{
    public ushort Index = 1;
    public ushort Power = 1;

    public UBooster()
    {
        Collision.Radius = 1;
    }
}