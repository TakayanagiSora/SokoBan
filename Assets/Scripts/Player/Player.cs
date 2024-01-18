using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private GameInputs _gameInputs = default;
    private MoveLogic _moveLogic = default;


    private void Awake()
    {
        _moveLogic = this.GetComponent<MoveLogic>();

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


    private void InitInput()
    {
        _gameInputs = new();
        _gameInputs.Enable();

        _gameInputs.Player.Move.started += OnMove;
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        _moveLogic.Move(context.ReadValue<Vector2>());
    }
}
