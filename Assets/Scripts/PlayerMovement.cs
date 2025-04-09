using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Vector3 direction = Vector3.zero.normalized;
    private Quaternion rotation;
    float hzInput;
    float vInput;
    [SerializeField] CharacterController controller;
    [SerializeField] CinemachineFreeLook freelookCamera;
    public Transform cameraTransform;
    
    float groundYOffset;
    public LayerMask groundMask;
    Vector3 spherePos;
    
    public bool playerIsClimbing;
    [SerializeField] public float climbSpeed;   
    
    [SerializeField] public float gravity;
    [SerializeField] public float jumpForce;
    [SerializeField] public float moveSpeed;
    [SerializeField] public float maxWalkSpeed;
    [SerializeField] public float maxFallSpeed;
    private bool isGrounded;
    // private bool jumped;
    [SerializeField] Vector3 velocity;
    [SerializeField] Vector3 finalPlayerMotion;
    
    Vector3 moveVelocity = Vector3.zero;
    [SerializeField] private float acceleration = 10f;
    [SerializeField] private float deceleration = 20f;
    
    private Vector3 currentDirection;
    private float currentSpeed;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        groundYOffset = controller.height - controller.radius + 0.15f;
        
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        velocity = Vector3.zero;
    }

    /* void Update()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            jumped = true;
        }
    } */

    void FixedUpdate()
    {
        // PlayerMove();
        // PlayerJump();
        // CalculatePlayerMotion();
        GetDirectionAndMove();
        VerticalMovement();
    }

    /* void PlayerMove()
    {
        hzInput = Input.GetAxis("Horizontal");
        vInput = Input.GetAxis("Vertical");

        velocity.x = hzInput * maxWalkSpeed;
        velocity.z = vInput * maxWalkSpeed;
    } */

    /* void PlayerJump()
    {
        if (jumped)
        {
            velocity.y = jumpForce;
            jumped = false;
        }
        
        else if (velocity.y < 0)
        {
            velocity.y += gravity * Time.fixedDeltaTime;
            velocity.y = Mathf.Max(maxFallSpeed, velocity.y);
        }

        // velocity.y = 0f;

        else if (isGrounded)
        {
            velocity.y = -0.1f;
        }
    } */

    /* void CalculatePlayerMotion()
    {
        finalPlayerMotion = velocity * Time.fixedDeltaTime;
        finalPlayerMotion = transform.TransformVector(finalPlayerMotion);

        controller.Move(finalPlayerMotion);
    } */

    void GetDirectionAndMove()
    {
        hzInput = Input.GetAxis("Horizontal");
        vInput = Input.GetAxis("Vertical");

        float cameraAngle = freelookCamera.m_XAxis.Value;
        rotation = Quaternion.Euler(0, cameraAngle, 0);

        Vector3 inputDirection = new Vector3(hzInput, 0, vInput).normalized;
        direction = rotation * inputDirection;

        /* if (direction.magnitude > 0.05f)
        {
            moveVelocity = Vector3.Lerp(moveVelocity, direction * moveSpeed, Time.fixedDeltaTime * acceleration);
        }
        else
        {
            moveVelocity = Vector3.Lerp(moveVelocity, Vector3.zero, Time.fixedDeltaTime * deceleration);
        }

        controller.Move(moveVelocity * Time.fixedDeltaTime);

        if (direction.magnitude > 0.05f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * 10f);
        } */
        
        // Move in relation to camera
        if (direction.magnitude >= 0.1f)
        {
            Vector3 camForward = cameraTransform.forward;
            camForward.y = 0;
            camForward.Normalize();
            Vector3 camRight = cameraTransform.right;
            camRight.y = 0;
            camRight.Normalize();

            Vector3 targetDir = camForward * direction.z + camRight * direction.x;
            currentDirection = Vector3.Lerp(currentDirection, targetDir, acceleration * Time.deltaTime);
            currentSpeed = Mathf.Lerp(currentSpeed, moveSpeed, acceleration * Time.deltaTime);
        }
        else
        {
            currentSpeed = Mathf.Lerp(currentSpeed, 0, deceleration * Time.deltaTime);
        }

        Vector3 move = currentDirection * currentSpeed;

        controller.Move((move + velocity) * Time.deltaTime);
    }
    
    bool IsGrounded()
    {
        spherePos = new Vector3(transform.position.x, transform.position.y - groundYOffset, transform.position.z);

        return Physics.CheckSphere(spherePos, controller.radius, groundMask);
    }

    void VerticalMovement()
    {
        if (playerIsClimbing)
        {
            velocity.y = climbSpeed * Time.fixedDeltaTime;
        }
        else if (!IsGrounded())
        {
            velocity.y += gravity * Time.fixedDeltaTime;
            // Debug.Log("Not Grounded");
        }
        else
        {
            if (Input.GetButtonDown("Jump"))
            {
                velocity.y += jumpForce * Time.fixedDeltaTime * 10;;
                Debug.Log("Jump");
            }
            else
            {
                velocity.y = -2f;
                // Debug.Log("Grounded");
            }
        }
        
        controller.Move(velocity * Time.fixedDeltaTime);
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Climb") && Input.GetButton("Climb"))
        {
            playerIsClimbing = true;
            Debug.Log("Player IN range of climbable object");
        }
        else
        {
            playerIsClimbing = false;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Climb"))
        {
            playerIsClimbing = false;
            Debug.Log("Player IN range of climbable object");
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        spherePos = new Vector3(transform.position.x, transform.position.y - groundYOffset, transform.position.z);
        Gizmos.DrawWireSphere(spherePos, controller.radius);
    }
}