using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] public float moveSpeed;
    public Vector3 direction;
    private Quaternion rotation;
    float hzInput;
    float vInput;
    [SerializeField] CharacterController controller;
    [SerializeField] CinemachineFreeLook freelookCamera;
    
    float groundYOffset;
    public LayerMask groundMask;
    Vector3 spherePos;

    [SerializeField] public float gravity;
    [SerializeField] public float jumpForce;
    Vector3 velocity;
    
    Vector3 moveVelocity = Vector3.zero;
    [SerializeField] private float acceleration = 10f;
    [SerializeField] private float deceleration = 20f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        groundYOffset = controller.height - controller.radius + 0.15f;
        
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void FixedUpdate()
    {
        GetDirectionAndMove();
        GravityAndJump();
    }

    void GetDirectionAndMove()
    {
        hzInput = Input.GetAxis("Horizontal");
        vInput = Input.GetAxis("Vertical");

        float cameraAngle = freelookCamera.m_XAxis.Value;
        rotation = Quaternion.Euler(0, cameraAngle, 0);

        Vector3 inputDirection = new Vector3(hzInput, 0, vInput).normalized;
        direction = rotation * inputDirection;

        if (direction.magnitude > 0.05f)
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
        }
    }


    bool IsGrounded()
    {
        spherePos = new Vector3(transform.position.x, transform.position.y - groundYOffset, transform.position.z);

        return Physics.CheckSphere(spherePos, controller.radius, groundMask);
    }

    void GravityAndJump()
    {
        if (!IsGrounded())
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        spherePos = new Vector3(transform.position.x, transform.position.y - groundYOffset, transform.position.z);
        Gizmos.DrawWireSphere(spherePos, controller.radius);
    }
}