using Server.Physics.Hood;
using Server.Physics.Hood.Components;

namespace Server.Physics
{
    public static class Physics
    {
        public static bool CheckCollisionRectangles(AtomicObject target, AtomicObject main)
        {
            if(!CheckActiveObject(target, main)) return false;
            
            return target.Transform.Position.X < main.Transform.Direction.X + main.Collision.Size.X &&
                   target.Transform.Position.X + target.Collision.Size.X > main.Transform.Direction.X &&
                   
                   target.Transform.Position.Y < main.Transform.Direction.Y + main.Collision.Size.Y &&
                   target.Transform.Position.Y + target.Collision.Size.Y > main.Transform.Direction.Y;
        }
        
        public static bool CheckCollisionCirclesInCircle(AtomicObject target, AtomicObject main)
        {
            if(!CheckActiveObject(target, main)) return false;
            
            var distance = main.Transform.Distance(target.Transform.Position);
            return distance > (main.Collision.Radius - target.Collision.Radius);
        }
        
        public static bool CheckCollisionCircles(AtomicObject target, AtomicObject main)
        {
            if(!CheckActiveObject(target, main)) return false;
            
            var distance = target.Transform.Distance(main.Transform.Position);
            return distance < (target.Collision.Radius + main.Collision.Radius);
        }

        public static bool CheckCollisionCircles(AtomicObject target, AtomicObject main, float offsetDistance)
        {
            if(!CheckActiveObject(target, main)) return false;
            
            var distance = target.Transform.Distance(main.Transform.Position);
            return distance < (target.Collision.Radius + (main.Collision.Radius + offsetDistance));
        }

        public static bool CheckCollisionCirclesDirection(AtomicObject target, AtomicObject main)
        {
            if(!CheckActiveObject(target, main)) return false;
            
            var distance = target.Transform.Distance(main.Transform.Direction);
            return distance < (target.Collision.Radius + main.Collision.Radius);
        }

        public static bool CheckCollisionInFront(AtomicObject target, AtomicObject main)
        {
            return CalculateCollisionInFront(target, main, 180);
        }
        
        public static bool CheckCollisionInFrontHalf(AtomicObject target, AtomicObject main)
        {
             return CalculateCollisionInFront(target, main, 90);
        }
        
        public static bool CheckCollisionInFrontQuarter(AtomicObject target, AtomicObject main)
        {
             return CalculateCollisionInFront(target, main, 45);
        }

        private static bool CalculateCollisionInFront(AtomicObject target, AtomicObject main, int sectorAngle)
        {
            if (!CheckActiveObject(target, main)) return false;
            
            var targetTransform = target.Transform;
            var mainTransform = main.Transform;
            
            var distance = targetTransform.Distance(mainTransform.Position);
            var distanceSquared = Math.Sqrt(distance);
            
            var combinedRadius = main.Collision.Radius + target.Collision.Radius;
            
            if (distanceSquared > combinedRadius * combinedRadius) return false;
            
            var magnitude = (float)Math.Sqrt(mainTransform.Forward.X * mainTransform.Forward.X + mainTransform.Forward.Y * mainTransform.Forward.Y);
            var normalizedDirX = mainTransform.Forward.X / magnitude;
            var normalizedDirY = mainTransform.Forward.Y / magnitude;
            
            var toTargetV2 = targetTransform.Position - mainTransform.Position;
            
            var magnitudeToOther = (float)Math.Sqrt(toTargetV2.X * toTargetV2.X * toTargetV2.Y * toTargetV2.Y);
            var normalizedToOtherX = toTargetV2.X / magnitudeToOther;
            var normalizedToOtherY = toTargetV2.Y / magnitudeToOther;
            
            var dotProduct = normalizedDirX * normalizedToOtherX + normalizedDirY * normalizedToOtherY;
            
            var sectorCosine = (float)Math.Cos(sectorAngle * Math.PI / 180.0);
            return dotProduct >= sectorCosine;
        }
        
        private static bool CheckCollisionLayers(AtomicObject target, AtomicObject main)
        {
            return false;
        }
        private static bool CheckActiveObject(AtomicObject target, AtomicObject main)
        {
            if (target.IsActiveObject == false) return false;
            if (main.IsActiveObject == false) return false;

            return true;
        }
    }
}