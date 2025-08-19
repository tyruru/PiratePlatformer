using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputReaderExperement : MonoBehaviour
{
    [SerializeField] private PlayerExperement _player;

    private PlayerInputAction _inputAction;

    private void Awake()
    {
        _inputAction = new PlayerInputAction();
        _inputAction.Enable();
    }

    private void OnEnable()
    {
        _inputAction.Player.Horizontal.performed += OnHorizontal;
        _inputAction.Player.Horizontal.canceled += OnHorizontal;
    }

    private void OnDisable()
    {
        _inputAction.Player.Horizontal.performed -= OnHorizontal;
        _inputAction.Player.Horizontal.canceled -= OnHorizontal;
    }

    private void OnHorizontal(InputAction.CallbackContext context)
    {
        _player.SetDirection(context.ReadValue<float>());
    }
}
