using UnityEngine;

public class MoveLogic : MonoBehaviour
{
    protected MapController _map = default;

    private void Awake()
    {
        _map = FindObjectOfType<MapController>();
    }

    /// <summary>
    /// 移動処理の実装
    /// </summary>
    /// <param name="dirType"></param>
    public virtual void Move(Vector2 inputDir)
    {
        MapIndexData moveDir = new(Mathf.RoundToInt(inputDir.x), Mathf.RoundToInt(inputDir.y));

        _map.MapUpdate(moveDir);
    }
}
