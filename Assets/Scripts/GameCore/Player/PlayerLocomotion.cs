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
        private readonly int _isSprinting = Animator.StringToHash("IsSprinting");

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        // Start is called before the first frame update
        void Start()
        {
            PlayerInputManager.Instance.SubscribeToMovementInputs(MovementInputsHandling);
            PlayerInputManager.Instance.SubscribeToSprintInput(SprintInputsHandling);
            
        }

        private void MovementInputsHandling(Vector2 inputs)
        {
            UpdateMovementAnimationParams(inputs);
        }
        private void SprintInputsHandling(bool _sprinting)
        {
            UpdateSprintAnimationParams(_sprinting);
        }

        private void UpdateMovementAnimationParams(Vector2 inputVector)
        {
            _animator.SetFloat(_inputX, inputVector.x);
            _animator.SetFloat(_inputY, inputVector.y);
        }
        private void UpdateSprintAnimationParams(bool isSprinting)
        {
            _animator.SetBool(_isSprinting,isSprinting);
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