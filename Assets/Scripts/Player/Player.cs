using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private GameInputs _gameInputs = default;
    private MoveLogic _moveLogic = default;


    private void Awake()
    {
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

        _gameInputs.Player.Move.started += Push;
    }

    private void Push(InputAction.CallbackContext context)
    {

    }
}
