using System.Numerics;

namespace Server.Room;

public class Bot : Unit
{
    private Vector2 _targetPosition = new();

    public void SetTargetPosition(Vector2 targetPos) => _targetPosition = targetPos;
    public Vector2 GetTargetPosition() => _targetPosition;
}