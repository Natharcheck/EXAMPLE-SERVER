namespace Server.Room.Interface;

public interface IMovementService
{
    public void Movement(ushort connectionId, ushort directionAngle);
}