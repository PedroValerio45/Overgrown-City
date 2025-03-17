using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    public int collectableID;
    bool collected = false;
    bool playerInRange = false;
    
    void Update()
    {
        
        if (playerInRange && !collected && Input.GetKeyDown(KeyCode.E))
        {
            collected = true;
            Debug.Log("Collected");
        }
        
        if (collected) { gameObject.SetActive(false); }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            Debug.Log("Player IN range of collectable" + collectableID);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            Debug.Log("Player OUT of range of collectable" + collectableID);
        }
    }
}
