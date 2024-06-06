using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Serialization;


namespace Gameplay.Player
{
    public class PlayerAiming : MonoBehaviour
    {
        public float turnSpeed = 15f;
        public float aimDuration = .3f;


        private Camera _mainCamera;

        [SerializeField] private BaseWeapon currentWeapon;
        [SerializeField] private Transform crosshairTargetTransform;
        [SerializeField] private Transform weaponParentTransform;
        [SerializeField] private Animator rigAnimController;

        private readonly int _holsterBoolHash = Animator.StringToHash("holster_weapon");
        
        private void Awake()
        {
            
        }

        // Start is called before the first frame update
        void Start()
        {
            _mainCamera = Camera.main;
            //Cursor.lockState = CursorLockMode.Confined;
        }

        private void Update()
        {
            if (currentWeapon)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    currentWeapon.StartFiring();
                }
                
                if(currentWeapon.isFiring)
                {
                    currentWeapon.UpdateFiring(Time.deltaTime);
                }

                currentWeapon.UpdateBulletsSimilation(Time.deltaTime);
                if (Input.GetMouseButtonUp(0))
                {
                    currentWeapon.StopFiring();
                }

                if (Input.GetKeyDown(KeyCode.X))
                {
                    ToggleWeaponHolster();
                }
            }
         
        }

        private void ToggleWeaponHolster()
        {
            if (rigAnimController)
            {
                bool isHolstered = rigAnimController.GetBool(_holsterBoolHash);
                rigAnimController.SetBool(_holsterBoolHash, !isHolstered);
            }
        }

        private void FixedUpdate()
        {
            float yawCamera = _mainCamera.transform.rotation.eulerAngles.y;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, yawCamera, 0),
                turnSpeed * Time.fixedDeltaTime);
        }
        
        public void EquipWeapon(BaseWeapon weapon, WeaponData weaponData = null)
        {
            if (currentWeapon)
            {
                Destroy(currentWeapon.gameObject);    
            }
            currentWeapon = weapon;
            if (currentWeapon)
            {
                currentWeapon.Init(crosshairTargetTransform, weaponParentTransform);
            }

            if (weaponData)
            {
                rigAnimController.CrossFade(weaponData.GetWeaponAnimationName(),0.1f);
            }
           
        }
    }
}