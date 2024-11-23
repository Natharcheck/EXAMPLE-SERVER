using System.Numerics;
using Server.Physics.Hood.Interface;

namespace Server.Physics.Hood.Components
{
    public enum CollisionLayer
    {
        None,
        Object,
        Ignore,
    }
    public class Collision : IComponent
    {
        public CollisionLayer MainLayer = CollisionLayer.None;
        
        public Vector2 Size = new Vector2();
        public float Radius { get;  set; }
    }
}