using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;


namespace Gameplay.Player
{
    public class PlayerAiming : MonoBehaviour
    {
        public float turnSpeed = 15f;
        public float aimDuration = .3f;

        [SerializeField] private Rig aimLayer;

        private Camera _mainCamera;

        [SerializeField] private BaseWeapon currentWeapon;

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

        private void LateUpdate()
        {
            if (aimLayer)
            {
                if (Input.GetMouseButton(1))
                {
                    aimLayer.weight += Time.deltaTime / aimDuration;
                }
                else
                {
                    aimLayer.weight -= Time.deltaTime / aimDuration;
                }
            }

            if (currentWeapon)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    currentWeapon.StartFiring();
                }

                if (Input.GetMouseButtonUp(0))
                {
                    currentWeapon.StopFiring();
                }
            }
        }
    }
}