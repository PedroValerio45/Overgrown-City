using System.Collections;
using UnityEngine;

public class DoorOpen : MonoBehaviour
{
    public promptE promptE;
    
    public bool doorOpen;
    private bool isPlayerInRange;

    [SerializeField] private float closedRotationY;
    private Quaternion closedRotation;
    private Quaternion openRotation;
    public float openSpeed = 2f;
    public bool doorOpenBackwards;
    
    private void Start()
    {
        promptE = FindObjectOfType<promptE>();
        
        closedRotationY = transform.rotation.eulerAngles.y;
        closedRotation = transform.rotation;

        if (doorOpenBackwards)
        {
            openRotation = Quaternion.Euler(0, closedRotationY + 80, 0);
        }
        else
        {
            openRotation = Quaternion.Euler(0, closedRotationY - 80, 0);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !doorOpen && isPlayerInRange)
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