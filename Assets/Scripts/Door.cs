using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour
{
    public promptE promptE;
    
    private bool doorOpen;
    private bool isPlayerInRange;

    private Quaternion closedRotation;
    private Quaternion openRotation;
    public float openSpeed = 2f;
    
    private void Start()
    {
        promptE = FindObjectOfType<promptE>();
        
        closedRotation = transform.rotation;
        openRotation = Quaternion.Euler(0, -90, 0);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !doorOpen &&  isPlayerInRange)
        {
            StartCoroutine(OpenDoor()); 
        }
    }

    private IEnumerator OpenDoor()
    {
        doorOpen = true;
        float time = 0f;
        while (time < 1f)
        {
            time += Time.deltaTime * openSpeed;
            transform.rotation = Quaternion.Lerp(closedRotation, openRotation, time);
            yield return null;
        }
        
        Debug.Log("Door opened");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            Debug.Log("Player IN door range");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            promptE.PromptE_Disable();
            Debug.Log("Player OUT OF door range");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (doorOpen) { promptE.PromptE_Disable(); }
            else { promptE.PromptE_Show(); }
        }
    }
}