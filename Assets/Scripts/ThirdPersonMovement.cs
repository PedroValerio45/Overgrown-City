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
    public float gravity = -20f;

    private CharacterController controller;
    private Vector3 velocity;
    private Vector3 currentDirection;
    private float currentSpeed;
    
    public bool isClimbing;
    public float climbSpeed = 10f; 

    private bool isJumping;
    private bool isGrounded;
    private float verticalVelocity;

    private bool isFrozen = false;
    private Transform lookTarget;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        
        //Cursor.visible = false;
        //Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (Input.GetButtonDown("Jump") && isGrounded && !isClimbing)
        {
            isJumping = true;
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
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

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
            currentDirection = Vector3.Lerp(currentDirection, targetDir, acceleration * Time.deltaTime);
            currentSpeed = Mathf.Lerp(currentSpeed, speed, acceleration * Time.deltaTime);
        }
        else
        {
            currentSpeed = Mathf.Lerp(currentSpeed, 0, deceleration * Time.deltaTime);
        }

        Vector3 move = currentDirection * currentSpeed;

        // Gravity
        if (!isClimbing)
        {
            velocity.y += gravity * Time.deltaTime;
        }

        controller.Move((move + velocity) * Time.deltaTime);

        // Rotate Model, not object
        if (currentDirection.magnitude > 0.1f)
        {
            Vector3 lookDirection = new Vector3(currentDirection.x, 0, currentDirection.z);
            if (lookDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
                characterModel.rotation = Quaternion.Slerp(characterModel.rotation, targetRotation, Time.deltaTime * 10f);
            }
        }
    }

    void VerticalMovement()
    {
        // Climb
        if (isClimbing)
        {
            velocity.y = climbSpeed * Time.fixedDeltaTime;
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
        if (other.CompareTag("Climb") && Input.GetButton("Climb"))
        {
            isClimbing = true;
            Debug.Log("Player IN range of climbable object");
        }
        else
        {
            isClimbing = false;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Climb"))
        {
            isClimbing = false;
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
            characterModel.rotation = Quaternion.Slerp(characterModel.rotation, targetRotation, Time.deltaTime * 5f);
        }
    }
}
