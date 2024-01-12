using UnityEngine;

public abstract class MoveLogic : MonoBehaviour
{
    /// <summary>
    /// 移動処理の実装
    /// </summary>
    /// <param name="dirType"></param>
    public abstract void Move(DirectionType dirType);
}
