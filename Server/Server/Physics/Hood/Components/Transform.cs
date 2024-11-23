using System.Numerics;
using Server.Physics.Hood.Interface;

namespace Server.Physics.Hood.Components
{
    public class Transform : IComponent
    {
        public Vector2 Position = new Vector2();
        public Quaternion Rotation = new Quaternion();

        public Vector2 Direction = new Vector2();

        public Vector2 Forward
        {
            get
            {
                var radians = Rotation.Y * (Math.PI / 180.0); 
                var directionX = (float)Math.Sin(radians); 
                var directionZ = (float)Math.Cos(radians);

                return new Vector2(directionX, directionZ);
            }
            set { }
        }

        public float Distance(Vector2 target)
        {
            var delta = Position - target;
            return (float)Math.Sqrt(delta.X * delta.X + delta.Y * delta.Y);
        }
        
        public void LookAt(Vector2 target)
        {
            var direction = target - Position;
        
            float angle = (float)Math.Atan2(direction.Y, direction.X); 
        
            Rotation.Y = angle * (180.0f / (float)Math.PI);
            Direction = direction; 
        }
        
        public void LookAt(ushort angle)
        {
            Rotation.Y = angle * (180.0f / (float)Math.PI);
        }

    }
}