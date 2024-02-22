using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float floorOffsetY;
    public float moveSpeed = 6f, rotateSpeed = 10f;

    public int animationFloatname;
    public SerializableDictionary<string, string> keys;
    Rigidbody rb;
    Animator anim;
    Vector3 moveDirection, inputVector;
    float inputAmount;
    Vector3 raycastFloorPos;
    Vector3 floorMovement;
    Vector3 gravity;
    Vector3 CombinedRaycast;


    //caching as it would be required
    Gameplay.Inputs.PlayerInputManager playerInputManager;


    PlayerAnimationManager playerAnimationManager;

    Vector3 normalizedVelocity;

    private void Awake()
    {
        playerInputManager = Gameplay.Inputs.PlayerInputManager.Instance;
        playerAnimationManager = GetComponent<PlayerAnimationManager>();

        playerInputManager.SubscribeToMovementInputs(InputHandling);
    }
    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        keys = new SerializableDictionary<string, string>()
        {
            { "asdasdf", "asdfgsdgfdgfdsgfds" },
            { "adssa", "asdf gsdgfdgfdsgfds" },
            { "asdasrewdf", "asdfgsdgfdgfdsgfds" }
        };
        keys["adssa"] = "jkf";
    }

    private void Update()
    {
        normalizedVelocity = rb.velocity.normalized;
        //playerAnimationManager.PlayerVelocityHandler(normalizedVelocity);
    }
    void TurnThePlayer()
    {
        // base movement on camera
        Vector3 combinedInput = CameraDirection(inputVector);

        moveDirection = new Vector3(combinedInput.x, 0, combinedInput.z).normalized;

        if (moveDirection.sqrMagnitude > 0.01f)
        {
            Quaternion rot = Quaternion.LookRotation(moveDirection);
            Quaternion targetRotation = Quaternion.Slerp(transform.rotation, rot, Time.fixedDeltaTime * inputAmount * rotateSpeed);
            transform.rotation = targetRotation;

        }
    }

    private void InputHandling(Vector2 _inputVector)
    {
        // reset movement
        //moveDirection = Vector3.zero;
        playerAnimationManager.PlayerVelocityHandler(_inputVector);
        inputVector = _inputVector;

        // make sure the input doesnt go negative or above 1;
        float inputMagnitude = Mathf.Abs(_inputVector.x) + Mathf.Abs(_inputVector.y);
        inputAmount = Mathf.Clamp01(inputMagnitude);


    }

    private Vector3 CameraDirection(Vector3 inputVector)
    {
        Vector3 cameraDir = (inputVector.x * Camera.main.transform.right) + (inputVector.y * Camera.main.transform.forward);
        cameraDir.y = 0f;
        return cameraDir;
    }

    /*    private void FixedUpdate()
        {
            // if not grounded , increase down force
            if (FloorRaycasts(0, 0, 0.6f) == Vector3.zero)
            {
                gravity += Physics.gravity.y * Time.fixedDeltaTime * Vector3.up;
            }

            moveDirection.Set(inputVector.x, 0, inputVector.y);

            // actual movement of the rigidbody + extra down force
            rb.velocity = (inputAmount * moveSpeed * moveDirection) + gravity;



            // rotate player to movement direction when there is a valid direction
            //TurnThePlayer();

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
    */
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
            if (hit.collider.gameObject.layer == 6)
                return hit.point;
        }
        return Vector3.zero;
    }
}