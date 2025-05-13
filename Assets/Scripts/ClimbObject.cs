using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbObject : MonoBehaviour
{
    public GameObject Player;
    public promptE promptE;
    
    [SerializeField] private bool playerInRange;
    public bool playerIsClimbing;

    void Start() { promptE = FindObjectOfType<promptE>(); }
    
    void Update()
    {
        if (playerInRange && Input.GetKey(KeyCode.E))
        {
            playerIsClimbing = true;
            // Player.transform.position = new Vector3(Player.transform.position.x, Player.transform.position.y + 5f * Time.deltaTime, Player.transform.position.z);
            // Player.transform.position = Vector3.Lerp(Player.transform.position, Vector3.up, Time.deltaTime);
            // Player.velocity = new Vector3(Player.velocity.x, verticalInput * climbSpeed, rb.velocity.z);
        }
        else
        {
            playerIsClimbing = false;
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Climb"))
        {
            playerInRange = true;
            promptE.PromptE_Show();
            Debug.Log("Player IN range of climbable object");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Climb"))
        {
            playerInRange = false;
            promptE.PromptE_Disable();
            Debug.Log("Player OUT of range of climbable object");
        }
    }
}