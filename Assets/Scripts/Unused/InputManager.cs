/* using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    Player_Controls playerControls;

    public Vector2 movementInput;
    public float horizontalInput;
    public float verticalInput;

    private void OnEnable()
    {
        if (playerControls == null)
        {
            playerControls = new Player_Controls();
            
            playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
        }
        
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    private void HandleMovementInput()
    {
        horizontalInput = movementInput.x;
        verticalInput = movementInput.y;
    }
} */