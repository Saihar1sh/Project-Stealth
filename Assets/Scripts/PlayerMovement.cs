using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using Gameplay.Inputs;

public class PlayerMovement : MonoBehaviour
{
    public float floorOffsetY;
    public float moveSpeed = 6f;
    public float rotateSpeed = 10f;

    public int animationFloatname;

    Rigidbody rb;
    //Animator anim;
    Vector3 moveDirection;
    float inputAmount;
    Vector3 raycastFloorPos;
    Vector3 floorMovement;
    Vector3 gravity;
    Vector3 CombinedRaycast;


    //caching as it would be required
    Gameplay.Inputs.PlayerInputManager playerInputManager;



    private void Awake()
    {
        playerInputManager = Gameplay.Inputs.PlayerInputManager.Instance;

        playerInputManager.SubscribeToMovementInputs((inputVector) => { InputHandling(inputVector); });
    }
    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //anim = GetComponent<Animator>();
    }

    private void Update()
    {
        Debug.Log("input: " + Input.GetAxis("Horizontal" + " " + Input.GetAxis("Vertical")));

    }
    private void InputHandling(Vector3 inputVector)
    {
        // reset movement
        //moveDirection = Vector3.zero;


        // base movement on camera
        Vector3 combinedInput = (inputVector.x * Camera.main.transform.right) + (inputVector.y * Camera.main.transform.forward);

        // normalize so diagonal movement isnt twice as fast, clear the Y so your character doesnt try to
        // walk into the floor/ sky when your camera isn't level

        moveDirection = new Vector3(combinedInput.x, 0, combinedInput.z).normalized;

        // make sure the input doesnt go negative or above 1;
        float inputMagnitude = Mathf.Abs(inputVector.x) + Mathf.Abs(inputVector.y);
        inputAmount = Mathf.Clamp01(inputMagnitude);

        // rotate player to movement direction when there is a valid direction
        if(moveDirection  != Vector3.zero)
        {
            Quaternion rot = Quaternion.LookRotation(moveDirection);
            Quaternion targetRotation = Quaternion.Slerp(transform.rotation, rot, Time.fixedDeltaTime * inputAmount * rotateSpeed);
            transform.rotation = targetRotation;

        }

        // handle animation blendtree for walking
        //anim.SetFloat(animationFloatname, inputAmount, 0.2f, Time.deltaTime);
    }

    private void FixedUpdate()
    {
        // if not grounded , increase down force
        if (FloorRaycasts(0, 0, 0.6f) == Vector3.zero)
        {
            gravity += Physics.gravity.y * Vector3.up * Time.fixedDeltaTime;
        }

        // actual movement of the rigidbody + extra down force
        rb.velocity = (moveDirection * moveSpeed * inputAmount) + gravity;

        // find the Y position via raycasts
        floorMovement = new Vector3(rb.position.x, FindFloor().y + floorOffsetY, rb.position.z);

        // only stick to floor when grounded
        if (FloorRaycasts(0, 0, 0.6f) != Vector3.zero && floorMovement != rb.position)
        {
            // move the rigidbody to the floor
            rb.MovePosition(floorMovement);
            gravity.y = 0;
        }

    }

    Vector3 FindFloor()
    {
        // width of raycasts around the centre of your character
        float raycastWidth = 0.25f;
        // check floor on 5 raycasts   , get the average when not Vector3.zero  
        int floorAverage = 1;

        CombinedRaycast = FloorRaycasts(0, 0, 1.6f);
        floorAverage += (getFloorAverage(raycastWidth, 0) + getFloorAverage(-raycastWidth, 0) + getFloorAverage(0, raycastWidth) + getFloorAverage(0, -raycastWidth));

        return CombinedRaycast / floorAverage;
    }

    // only add to average floor position if its not Vector3.zero
    int getFloorAverage(float offsetx, float offsetz)
    {

        if (FloorRaycasts(offsetx, offsetz, 1.6f) != Vector3.zero)
        {
            CombinedRaycast += FloorRaycasts(offsetx, offsetz, 1.6f);
            return 1;
        }
        else
            return 0;
    }


    Vector3 FloorRaycasts(float offsetx, float offsetz, float raycastLength)
    {
        // move raycast
        raycastFloorPos = transform.TransformPoint(0 + offsetx, 0 + 0.5f, 0 + offsetz);

        Debug.DrawRay(raycastFloorPos, Vector3.down, Color.magenta);
        if (Physics.Raycast(raycastFloorPos, -Vector3.up, out RaycastHit hit, raycastLength))
        {
            return hit.point;
        }
        else 
            return Vector3.zero;
    }
}