using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Gameplay.Inputs
{
    public class PlayerInputManager : MonoGenericLazySingleton<PlayerInputManager>
    {
        private PlayerControls _playerControls;

        private Action<Vector2> _movementAction = default;

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
        
#region Private Methods
        private void PlayerControlsInit()
        {
            _playerControls = new PlayerControls();

            _playerControls.Player.Move.started += ReadMovementAndReturnValue;
            _playerControls.Player.Move.performed += ReadMovementAndReturnValue;
            _playerControls.Player.Move.canceled += ReadMovementAndReturnValue;
        }

        private void ReadMovementAndReturnValue(InputAction.CallbackContext ctx)
        {
            Vector3 inputVector;
            Debug.Log(inputVector = ctx.ReadValue<Vector2>());
            _movementAction.Invoke(inputVector);
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