using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Gameplay.Inputs
{
    public class PlayerInputManager : MonoGenericLazySingleton<PlayerInputManager>
    {
        PlayerControls playerControls;

        private Action<Vector2> MovementAction = default;

        protected override void Awake()
        {
            base.Awake();

            PlayerControlsInit();
        }

        public void SubscribeToMovementInputs(System.Action<Vector2> mvtActionFunc)
        {
            MovementAction += mvtActionFunc;
        }
        public void UnSubscribeToMovementInputs(System.Action<Vector2> mvtActionFunc)
        {
            MovementAction -= mvtActionFunc;
        }

        private void PlayerControlsInit()
        {
            playerControls = new();

            playerControls.Player.Move.started += (ctx) => { ReadMovementAndReturnValue(ctx); };
            playerControls.Player.Move.performed += (ctx) => { ReadMovementAndReturnValue(ctx); };
            playerControls.Player.Move.canceled += (ctx) => { ReadMovementAndReturnValue(ctx); };
        }

        private void ReadMovementAndReturnValue(InputAction.CallbackContext ctx)
        {
            Vector3 inputVector;
            Debug.Log(inputVector = ctx.ReadValue<Vector2>());
            MovementAction.Invoke(inputVector);
        }

        private void OnEnable()
        {
            if (playerControls == null)
                PlayerControlsInit();

            playerControls.Enable();
        }
        private void OnDisable()
        {
            playerControls?.Disable();
        }

    }
}