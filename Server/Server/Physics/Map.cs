using System.Numerics;

namespace Server.Physics;

public class Map()
{
    public string Name = "CollisionMap";
    
    public List<Vector2> BoxPositions = new();
    public List<Vector2> RockPositions = new();
    public List<Vector2> SeaweedPositions = new();

    public void SetPositions(ushort blockId, List<Vector2> positions)
    {
        switch (blockId)
        {
            case 1 : BoxPositions = positions; break;
            case 2 : RockPositions = positions; break;
            case 3 : SeaweedPositions = positions; break;
        }
    }
}