using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ShGames.Gameplay.Inputs
{
    public class PlayerInputManager : MonoGenericLazySingleton<PlayerInputManager>
    {
        private PlayerControls _playerControls;

        private Action<Vector2> _movementAction = default;
        private Action<bool> _sprintAction = default;

        protected override void Awake()
        {
            base.Awake();

            PlayerControlsInit();
        }

        public void SubscribeToMovementInputs(System.Action<Vector2> mvtActionFunc)
        {
            _movementAction += mvtActionFunc;
        }
        public void UnSubscribeToMovementInputs(System.Action<Vector2> mvtActionFunc)
        {
            _movementAction -= mvtActionFunc;
        }
        
        public void SubscribeToSprintInput(System.Action<bool> sprintActionFunc)
        {
            _sprintAction += sprintActionFunc;
        }
        public void UnSubscribeToSprintInput(System.Action<bool> sprintActionFunc)
        {
            _sprintAction -= sprintActionFunc;
        }
        
#region Private Methods
        private void PlayerControlsInit()
        {
            _playerControls = new PlayerControls();

            _playerControls.Player.Move.started += ReadMovementAndReturnValue;
            _playerControls.Player.Move.performed += ReadMovementAndReturnValue;
            _playerControls.Player.Move.canceled += ReadMovementAndReturnValue;
            
            _playerControls.Player.Sprint.started += ReadSprintAndReturnValue;
            _playerControls.Player.Sprint.performed += ReadSprintAndReturnValue;
            _playerControls.Player.Sprint.canceled += ReadSprintAndReturnValue;
        }

        private void ReadMovementAndReturnValue(InputAction.CallbackContext ctx)
        {
            Vector3 inputVector;
            Debug.Log(inputVector = ctx.ReadValue<Vector2>());
            _movementAction.Invoke(inputVector);
        }
        private void ReadSprintAndReturnValue(InputAction.CallbackContext ctx)
        {
            bool isSprinting = ctx.ReadValueAsButton();
            Debug.Log("isSprinting: " + isSprinting);
            _sprintAction.Invoke(isSprinting);
        }

        private void OnEnable()
        {
            if (_playerControls == null)
                PlayerControlsInit();

            _playerControls?.Enable();
        }
        private void OnDisable()
        {
            _playerControls?.Disable();
        }
        
        #endregion
    }
}