using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    /* InputManager inputManager;
    
    Vector3 moveDirection;
    Transform cameraObject;
    Rigidbody playerRigidbody;

    public float movementSpeed = 7;
    public float rotationSpeed = 15;

    public void HandleMovement()
    {
        moveDirection = cameraObject.forward * inputManager.verticalInput;
        moveDirection = moveDirection + cameraObject.right * inputManager.horizontalInput;
        moveDirection.Normalize();
        moveDirection.y = 0;
        moveDirection *= movementSpeed;

        Vector3 movementVelocity = moveDirection;
        playerRigidbody.velocity = movementVelocity;
    }

    public void HandleRotation()
    {
        Vector3 targetDirection = Vector3.zero;
        
        targetDirection = cameraObject.foward * InputManager.verticalInput;
        targetDirection += cameraObject.right * InputManager.horizontalInput;
        targetDirection.Normalize();
        targetDirection.y = 0;
        
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        
        transform.rotation = playerRotation;
    } */

    /* PlayerInput playerInput;
    Rigidbody rb;
    // CharacterController charControl;
    InputAction moveAction;
    InputAction jumpAction;

    public float moveSpeed = 5;
    public float gravity = -10;
    public float jumpForce = 50;
    bool jump = false;
    Vector3 velocity;
    
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody>();
        // charControl = GetComponent<CharacterController>();
        moveAction = playerInput.actions.FindAction("Movement");
        jumpAction = playerInput.actions.FindAction("Jump");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
        
        MovePlayer();
        // Jump();
    }

    void MovePlayer()
    {
        Debug.Log(moveAction.ReadValue<Vector2>());
        
        Vector2 direction = moveAction.ReadValue<Vector2>();
        transform.position += new Vector3(direction.x, 0, direction.y) * moveSpeed * Time.deltaTime;
    }

    /* void Jump()
    {
        jump = jumpAction.ReadValue<bool>();
        
        // velocity.y += gravity * Time.deltaTime;
        if (jump)
        {
            rb.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
        }
        // transform.position += new Vector3(0, velocity.y, 0) * Time.deltaTime;
        // charControl.Move(velocity * Time.deltaTime);
    } */

    public float moveSpeed = 3;
    public Vector3 dir;
    float hzInput;
    float vInput;
    CharacterController controller;
    
    public float groundYOffset;
    public LayerMask groundMask;
    Vector3 spherePos;

    public float gravity = -10f;
    Vector3 velocity;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        GetDirectionAndMove();
        Gravity();
    }

    void GetDirectionAndMove()
    {
        hzInput = Input.GetAxis("Horizontal");
        vInput = Input.GetAxis("Vertical");
        
        dir = transform.forward * vInput + transform.right * hzInput;
        
        controller.Move(dir * moveSpeed * Time.deltaTime);
    }

    bool IsGrounded()
    {
        spherePos = new Vector3(transform.position.x, transform.position.y * groundYOffset, transform.position.z);
        
        if (Physics.CheckSphere(spherePos, controller.radius - 0.05f, groundMask)) return true;
        
        return false;
    }

    void Gravity()
    {
        if (!IsGrounded()) velocity.y += gravity * Time.deltaTime;
        else if (velocity.y < 0) velocity.y += -2;
        
        controller.Move(velocity * Time.deltaTime);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(spherePos, controller.radius - 0.05f);
    }
}