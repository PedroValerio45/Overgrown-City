using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorWrongSide : MonoBehaviour
{
    // EVERYTHING AUDIO HAS TO BE ADDED MANUALLY IN INSPECTOR FOR EVERY INSTANCE OF THIS FILE
    public AudioSource audioSourceHazardsAndDoor;
    public AudioClip doorStuckSound;
    
    public promptE promptE;
    public DoorOpen doorOpen;
    
    private bool doorIsOpen;
    private bool isPlayerInRange;
 
    private void Start()
    {
        promptE = FindObjectOfType<promptE>();
    }
    
    void Update()
    {
        doorIsOpen = doorOpen.doorOpen;
        
        if (Input.GetKeyDown(KeyCode.E) && !doorIsOpen && isPlayerInRange)
        {
            audioSourceHazardsAndDoor.clip = doorStuckSound;
            audioSourceHazardsAndDoor.Play();
            
            Debug.Log("There seems to be something blocking the way... Maybe it can be openned from the other side?"); // TO BE ADDED
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            Debug.Log("Player IN door wrong side");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            promptE.PromptE_Disable();
            Debug.Log("Player OUT OF door wrong side");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (doorIsOpen) { promptE.PromptE_Disable(); }
            else { promptE.PromptE_Show(); }
        }
    }
}
