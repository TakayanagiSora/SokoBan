using UnityEngine;

public abstract class MoveLogic : MonoBehaviour
{
    /// <summary>
    /// �ړ������̎���
    /// </summary>
    /// <param name="dirType"></param>
    public abstract void Move(DirectionType dirType);
}
