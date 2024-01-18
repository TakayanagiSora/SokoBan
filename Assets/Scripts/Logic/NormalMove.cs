using UnityEngine;

public class NormalMove : MoveLogic
{
    public override void Move(Vector2 inputDir)
    {
        (int y, int x) moveDir = (Mathf.RoundToInt(inputDir.y), Mathf.RoundToInt(inputDir.x));

        _map.MapUpdate(moveDir);
    }
}
