using Server.Physics.Hood.Components;

namespace Server.Physics.Hood
{
    public abstract class AtomicObject
    {
        public readonly Transform Transform;
        public readonly Collision Collision;
        public bool IsActiveObject { get; private set; }

        public delegate void WhenEnableObject();
        public delegate void WhenDisableObject();

        public event WhenEnableObject OnEnableObject;
        public event WhenDisableObject OnDisableObject;

        public AtomicObject()
        {
            Transform = new Transform();
            Collision = new Collision();
            
            SetActive(true);
        }
        
        public void SetActive(bool isActive)
        {
            var onActive = isActive ? OnEnable() : OnDisable();
        }
        
        protected virtual bool OnEnable()
        {
            OnEnableObject?.Invoke();
            return IsActiveObject = true;
        }
        
        protected virtual bool OnDisable()
        {
            OnDisableObject?.Invoke();
            return IsActiveObject = false;
        }
    }
}