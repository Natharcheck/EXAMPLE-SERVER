using System.Numerics;
using Server.Physics.Hood;

namespace Server.Room;

public class Spot : AtomicObject
{
    public float Range;

    public Unit Owner;
    private List<Unit> _units;

    public Spot(Unit owner, Vector2 position, float range)
    {
        Transform.Position = position;
        Collision.Radius = range;
        Range = range;

        Owner = owner;

        _units = new List<Unit>();
    }

    public void SetUnits(List<Unit> units) => _units = units;

    public void SetUnits(List<Player> players)
    {
        _units = new List<Unit>();
        _units.AddRange(players);
    }

    public void SetTarget()
    {
        foreach (var unit in _units)
        {
            if (Physics.Physics.CheckCollisionCircles(unit, this))
            {
                Owner.SetTarget(unit);

                if (Physics.Physics.CheckCollisionCircles(unit, Owner))
                {
                    Owner.IsMove = false;
                    Owner.AbilityCastHandler.OnCast(0);
                }
                else
                {
                    Owner.IsMove = true;
                }
            }
            else
            {
                Owner.IsMove = true;
                Owner.SetTarget(this);
            }
        }
    }

}