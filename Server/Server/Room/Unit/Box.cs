using System.Numerics;
using Server.Room;
using Server.Room.Componets;

namespace Server.Physics;

public sealed class Box : Unit
{
    public Box()
    {
        Initialize(null!, new UHealth(100, 100, 0));
        AddBooster(new UBooster());
    }
    
    public override void OnDead()
    {
        
    }
}