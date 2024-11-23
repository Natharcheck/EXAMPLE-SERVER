using Microsoft.Extensions.DependencyInjection;
using Server.Physics.Hood;
using Server.Physics.Hood.Components;
using Server.Room;
using Server.Room.Componets;

namespace Server.Physics;

public class CollisionMap
{
    private List<Block> _blocks;
    private List<UBooster> _boosters;
    private List<Unit> _units;

    private Map _map = ServiceContainer.Provider.GetRequiredService<Map>();

    public CollisionMap()
    {
        _blocks = new();
        _units = new();
        _boosters = new();
        
        CreateMapCollisions();
    }

    private void CreateMapCollisions()
    {
        var radius = 0.07f;
        var index = 10000;
        
        foreach (var position in _map.BoxPositions)
        {
            index++;
            
            var block = new Box()
            {
                Index = (ushort)index,
                Transform = { Position = position},
                Collision =
                {
                    Radius = radius,
                    MainLayer = CollisionLayer.Object,
                }
            };
            
            _units.Add(block);
        }
        
        foreach (var position in _map.RockPositions)
        {
            var block = new Rock(1)
            {
                Transform = { Position = position},
                Collision =
                {
                    Radius = radius,
                    MainLayer = CollisionLayer.Object,
                }
            };
            
            _blocks.Add(block);
        }
        
        foreach (var position in _map.SeaweedPositions)
        {
            var block = new Grass(2)
            {
                Transform = { Position = position},
                Collision =
                {
                    Radius = radius,
                    MainLayer = CollisionLayer.Ignore,
                }
            };
            
            _blocks.Add(block);
        }
    }

    public bool GetRectanglesCollision(AtomicObject main)
    {
        foreach (var collision in _blocks)
        {
            if (collision.Type == 1)
            {
                if (Physics.CheckCollisionRectangles(collision, main))
                {
                    return true;
                }   
            }
        }

        return false;
    }

    public bool GetCirclesCollision(AtomicObject main)
    {
        foreach (var collision in _blocks)
        {
            if (collision.Type == 1)
            {
                if (Physics.CheckCollisionCircles(collision, main))
                {
                    return true;
                }
            }
        }

        return false;
    }

    public bool GetCirclesCollisionDirection(AtomicObject main)
    {
        foreach (var collision in _blocks)
        {
            if (collision.Type == 1)
            {
                if (Physics.CheckCollisionCirclesDirection(collision, main))
                {
                    return true;
                }
            }
            else
            {
                if (Physics.CheckCollisionCircles(collision, main))
                {
                    //Отправка невидимости
                }
            }
        }

        return false;
    }

    public bool GetCirclesCollisionBoosters(AtomicObject main, out UBooster booster)
    {
        foreach (var collision in _boosters)
        {
            if (Physics.CheckCollisionCircles(collision, main))
            {
                booster = collision;
                return true;
            }
        }

        booster = null;
        return false;
    }

    public List<Unit> GetUnits()
    {
        for (byte i = 0; i < _units.Count; i++)
        {
            _units[i].Index = (byte)(i + 1);
        }
        
        return _units;
    }

    public void AddBooster(UBooster booster)
    {
        _boosters.Add(booster);
    }
    
    public void RemoveBooster(UBooster booster)
    {
        _boosters.Remove(booster);
    }
}