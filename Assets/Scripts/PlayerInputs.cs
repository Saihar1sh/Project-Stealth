using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputs : MonoBehaviour
{
    public static PlayerInputs Instance { get; private set; }

    PlayerControls playerControls;


    public Action<Vector2> MovementAction;

    private void Awake()
    {
        Instance = this;
        playerControls = new();
        //playerInput.PlayerMovement.Sprint.performed += (ctx) => { Debug.Log(ctx.action.name); };
    }

   public Vector2 GetMovementInput()
    {
        Vector3 inputVector = Vector2.zero;


        playerControls.Player.Move.performed += (ctx) => { inputVector = ReadMovementAndReturnValue(ctx); };
        playerControls.Player.Move.canceled += (ctx) => { inputVector = ReadMovementAndReturnValue(ctx); };

        return inputVector;
    }

    private Vector3 ReadMovementAndReturnValue(InputAction.CallbackContext ctx)
    {
        Vector3 inputVector;
        Debug.Log(inputVector = ctx.ReadValue<Vector2>()); MovementAction.Invoke(inputVector);
        return inputVector;
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }
    private void OnDisable()
    {
        playerControls.Disable();
    }

}
