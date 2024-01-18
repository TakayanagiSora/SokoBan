using UnityEngine;

public abstract class MoveLogic : MonoBehaviour
{
    protected Map _map = default;

    private void Awake()
    {
        _map = FindObjectOfType<Map>();
    }

    /// <summary>
    /// �ړ������̎���
    /// </summary>
    /// <param name="dirType"></param>
    public abstract void Move(Vector2 inputDir);
}
