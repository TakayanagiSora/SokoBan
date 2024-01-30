using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private GameInputs _gameInputs = default;
    private MapController _mapController = default;


    private void Awake()
    {
        _mapController = FindObjectOfType<MapController>();

        InitInput();
    }

    private void OnEnable()
    {
        _gameInputs?.Enable();
    }

    private void OnDisable()
    {
        _gameInputs?.Disable();
    }


    /// <summary>
    /// InputSystemの初期化、イベント購読
    /// </summary>
    private void InitInput()
    {
        _gameInputs = new();
        _gameInputs.Enable();

        _gameInputs.Player.Move.started += OnMove;
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        Vector2 inputDir = context.ReadValue<Vector2>();

        // コントローラー用：4方向のうち最も近い方向に補正
        MapIndexData moveDir = new(Mathf.RoundToInt(inputDir.y), Mathf.RoundToInt(inputDir.x));

        // 移動＝マップの更新
        _mapController.MapUpdate(moveDir);
    }
}
