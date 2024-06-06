using System;
using System.Collections;
using System.Collections.Generic;
using Gameplay.Inputs;
using UnityEngine;

namespace Gameplay.Player
{
    public class PlayerLocomotion : MonoBehaviour
    {
        private Animator _animator;

        private readonly int _inputX = Animator.StringToHash("InputX");
        private readonly int _inputY = Animator.StringToHash("InputY");

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        // Start is called before the first frame update
        void Start()
        {
            PlayerInputManager.Instance.SubscribeToMovementInputs(MovementInputsHandling);
        }

        private void MovementInputsHandling(Vector2 inputs)
        {
            UpdateAnimationParams(inputs);
        }

        private void UpdateAnimationParams(Vector2 inputVector)
        {
            _animator.SetFloat(_inputX, inputVector.x);
            _animator.SetFloat(_inputY, inputVector.y);
        }

        private void OnDisable()
        {
            PlayerInputManager.Instance.UnSubscribeToMovementInputs(MovementInputsHandling);
        }

        private void OnDestroy()
        {
            //PlayerInputManager.Instance?.UnSubscribeToMovementInputs(MovementInputsHandling);
        }
    }
}