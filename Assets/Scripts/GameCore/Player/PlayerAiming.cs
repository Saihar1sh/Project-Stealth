using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Player
{
    public class PlayerAiming : MonoBehaviour
    {
        public float turnSpeed = 15f;

        private Camera _mainCamera;

        // Start is called before the first frame update
        void Start()
        {
            _mainCamera = Camera.main;
        }

        private void FixedUpdate()
        {
            float yawCamera = _mainCamera.transform.rotation.eulerAngles.y;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, yawCamera, 0),
                turnSpeed * Time.fixedDeltaTime);
        }
    }
}