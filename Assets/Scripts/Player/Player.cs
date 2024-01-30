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
    /// InputSystem�̏������A�C�x���g�w��
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

        // �R���g���[���[�p�F4�����̂����ł��߂������ɕ␳
        MapIndexData moveDir = new(Mathf.RoundToInt(inputDir.y), Mathf.RoundToInt(inputDir.x));

        // �ړ����}�b�v�̍X�V
        _mapController.MapUpdate(moveDir);
    }
}
