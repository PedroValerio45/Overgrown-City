using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbObject : MonoBehaviour
{
    public GameObject Player;
    
    private bool playerInRange;
    
    void Update()
    {
        if (playerInRange && Input.GetKey(KeyCode.E))
        {
            Player.transform.position = new Vector3(Player.transform.position.x, Player.transform.position.y + 1f, Player.transform.position.z);
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            Debug.Log("Player IN range of climbable object");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            Debug.Log("Player IN range of climbable object");
        }
    }
}