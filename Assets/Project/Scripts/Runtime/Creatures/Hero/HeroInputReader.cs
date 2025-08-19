using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Project.Scripts
{
    public sealed class HeroInputReader : MonoBehaviour
    {
        [FormerlySerializedAs("_player")] [SerializeField] private Hero _hero;

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
            _inputAction.Player.Interact.canceled += OnInteract;
            _inputAction.Player.Attack.canceled += OnAttack;
            _inputAction.Player.Throw.canceled += OnTrow;
            _inputAction.Player.Throw.started += OnTrow;
            _inputAction.Player.NextItem.started += OnUse;
            _inputAction.Player.UsePerk.performed += OnUsePerk;
            _inputAction.Player.OnToggleFlashlight.started += OnToggleFlashlight;
        }

        private void OnDisable()
        {
            _inputAction.Player.Horizontal.performed -= OnHorizontal;
            _inputAction.Player.Horizontal.canceled -= OnHorizontal;
            _inputAction.Player.Interact.canceled -= OnInteract;
            _inputAction.Player.Attack.canceled -= OnAttack;
            _inputAction.Player.Throw.started -= OnTrow;
            _inputAction.Player.Throw.canceled -= OnTrow;
            _inputAction.Player.NextItem.started -= OnUse;
            _inputAction.Player.UsePerk.performed -= OnUsePerk;
            _inputAction.Player.OnToggleFlashlight.performed -= OnToggleFlashlight;
        }

        private void OnHorizontal(InputAction.CallbackContext context)
        {
            _hero.SetDirection(context.ReadValue<Vector2>());
        }
        
        private void OnInteract(InputAction.CallbackContext context)
        {
            if (context.canceled)
            {
                _hero.Interact();
            }
        }
        private void OnAttack(InputAction.CallbackContext context)
        {
            if (context.canceled)
            {
                _hero.Attack();
            }
        }

        private void OnTrow(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                _hero.StartThrowing();
            }

            if (context.canceled)
            {
                _hero.UseInventory();
            }
        }
        
        private void OnUse(InputAction.CallbackContext context)
        {
            _hero.NextItem();
        }
        
        public void SetLock(bool isLocked)
        {
            if(isLocked) 
                _inputAction.Disable();
            else
                _inputAction.Enable();
        }

        private void OnUsePerk(InputAction.CallbackContext contxext)
        {
            _hero.UsePerk();
        }   
        
        private void OnToggleFlashlight(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                _hero.ToggleFlashlight();
            }
        }
    }
}
