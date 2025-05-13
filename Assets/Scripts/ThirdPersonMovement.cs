using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class ThirdPersonMovement : MonoBehaviour
{
    public Transform characterModel;
    
    public Transform cameraTransform; // MAIN CAMERA OF EACH SCENE (NEEDS TO BE MANUALLY ASSIGNED)
    public float speed = 12f;
    public float acceleration = 10f;
    public float deceleration = 8f;
    public float jumpHeight = 3f;
    public float gravity = -30f;

    private Vector3 move;

    private CharacterController controller;
    private Vector3 velocity;
    private Vector3 currentDirection;
    private float currentSpeed;
    
    public bool isClimbing;
    public bool inClimbRange;
    public bool climbingUp;
    public bool climbingDown;
    public float climbSpeed = 250f; 

    private bool isJumping;
    private bool isGrounded;
    private float verticalVelocity;

    private bool isFrozen;
    private Transform lookTarget;

    void Start()
    {
        controller = FindObjectOfType<CharacterController>();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (Input.GetButtonDown("Jump") && isGrounded && !isClimbing) { isJumping = true; }
        
        if (Input.GetButtonDown("Climb") && inClimbRange)
        {
            move = Vector3.zero;
            velocity = Vector3.zero;
            
            if (!isClimbing)
            {
                isClimbing = true;
            }
            else
            {
                isClimbing = false;
            }
        }

        if (Input.GetKey(KeyCode.W) && isClimbing)
        {
            climbingUp = true;
        }
        else if (Input.GetKeyUp(KeyCode.W) && isClimbing)
        {
            climbingUp = false;
            move = Vector3.zero;
            velocity = Vector3.zero;
        }

        if (Input.GetKey(KeyCode.S) && isClimbing)
        {
            climbingDown = true;
        }
        else if (Input.GetKeyUp(KeyCode.S) && isClimbing)
        {
            climbingDown = false;
            move = Vector3.zero;
            velocity = Vector3.zero;
        }
    }
    
    void FixedUpdate()
    {
        if (isFrozen)
        {
            FaceLookTarget();
            return;
        }

        HorizontalMovementAndRotation();
        VerticalMovement();
    }

    void HorizontalMovementAndRotation()
    {
        // Is Grounded
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0 && !isClimbing)
        {
            velocity.y = -2f;
        }

        if (!isClimbing)
        {
            // Gravity
            velocity.y += gravity * Time.fixedDeltaTime;
            
            // Input
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");

            Vector3 inputDir = new Vector3(horizontal, 0, vertical).normalized;

            // Move in relation to camera
            if (inputDir.magnitude >= 0.1f)
            {
                Vector3 camForward = cameraTransform.forward;
                camForward.y = 0;
                camForward.Normalize();
                Vector3 camRight = cameraTransform.right;
                camRight.y = 0;
                camRight.Normalize();

                Vector3 targetDir = camForward * inputDir.z + camRight * inputDir.x;
                currentDirection = Vector3.Lerp(currentDirection, targetDir, acceleration * Time.fixedDeltaTime);
                currentSpeed = Mathf.Lerp(currentSpeed, speed, acceleration * Time.fixedDeltaTime);
            }
            else
            {
                currentSpeed = Mathf.Lerp(currentSpeed, 0, deceleration * Time.fixedDeltaTime);
            }

            move = currentDirection * currentSpeed;
        }
        controller.Move((move + velocity) * Time.fixedDeltaTime);

        // Rotate Model, not object
        if (currentDirection.magnitude > 0.1f)
        {
            Vector3 lookDirection = new Vector3(currentDirection.x, 0, currentDirection.z);
            if (lookDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
                characterModel.rotation = Quaternion.Slerp(characterModel.rotation, targetRotation, Time.fixedDeltaTime * 10f);
            }
        }
    }

    void VerticalMovement()
    {
        // Climb
        if (isClimbing)
        {
            // Climb up and down
            if (climbingUp)
            {
                velocity.y = climbSpeed * Time.fixedDeltaTime;
                Debug.Log("W FUCKING WORK");
            }
            else if (climbingDown)
            {
                velocity.y = -climbSpeed * Time.fixedDeltaTime;
                Debug.Log("S FUCKING WORK");
            }
        }
        else
        {
            // Jump
            if (isJumping)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                isJumping = false;
            }
        }
    }
    
    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Climb"))
        {
            inClimbRange = true;
            
            Debug.Log("Player IN range of climbable object");
        }
        else
        {
            isClimbing = false;
            inClimbRange = false;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Climb"))
        {
            isClimbing = false;
            inClimbRange = false;
            climbingUp = false;
            climbingDown = false;
            Debug.Log("Player OUT range of climbable object");
        }
    }

    public void FreezePlayer(Transform target)
    {
        isFrozen = true;
        lookTarget = target;
        currentDirection = Vector3.zero;
        currentSpeed = 0f;
        velocity = Vector3.zero;
    }

    public void UnfreezePlayer()
    {
        isFrozen = false;
        lookTarget = null;
    }

    private void FaceLookTarget()
    {
        if (lookTarget == null) return;

        Vector3 directionToTarget = (lookTarget.position - transform.position).normalized;
        directionToTarget.y = 0;

        if (directionToTarget != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
            characterModel.rotation = Quaternion.Slerp(characterModel.rotation, targetRotation, Time.fixedDeltaTime * 5f);
        }
    }
}