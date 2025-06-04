using System.Collections;
using UnityEngine;

public class TNTCollectable : MonoBehaviour
{
    // EVERYTHING AUDIO HAS TO BE ADDED MANUALLY IN INSPECTOR FOR EVERY INSTANCE OF THIS FILE
    public AudioSource audioSourceHazards;
    public AudioSource audioSourcePlayerOthers;
    public AudioClip explosionSound;
    public AudioClip itemSound;
    
    public promptE promptE;
    public GameObject tntModel;
    public BuildingFall buildingFall;
    public GameObject semiTransparent;
    public GameObject fog;

    private bool collected;
    private bool placedDown;
    private bool playerInRange;
    private bool playerInZone;
    private bool tntPlaced;
    private bool buildingHasFallen;
    private bool readyToDetonate;

    private Transform playerTransform;
    private Transform tntZoneTransform;

    public float floatHeightAboveHead;
    public float rotationSpeed;

    private void Awake()
    {
        promptE = FindObjectOfType<promptE>();
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
    }

    void Update()
    {
        if (!placedDown)
        {
            transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime, Space.World);
        }

        if (collected && playerTransform != null && !placedDown)
        {
            transform.position = playerTransform.position + new Vector3(0, floatHeightAboveHead, 0);
        }

        // Pick up TNT
        if (playerInRange && !collected && Input.GetKeyDown(KeyCode.E))
        {
            audioSourcePlayerOthers.pitch = 1f;
            audioSourcePlayerOthers.clip = itemSound;
            audioSourcePlayerOthers.Play();
            
            collected = true;
            placedDown = false;
            promptE.PromptE_Disable();
        }

        // Place TNT in the zone
        if (collected && playerInZone && Input.GetKeyDown(KeyCode.E))
        {
            audioSourcePlayerOthers.pitch = 1f;
            audioSourcePlayerOthers.clip = itemSound;
            audioSourcePlayerOthers.Play();
            
            DropTNTToZone();
        }

        // Detonate if placed and ready
        if (tntPlaced && readyToDetonate && playerInZone && Input.GetKeyDown(KeyCode.E) && !buildingHasFallen)
        {
            audioSourceHazards.clip = explosionSound;
            audioSourceHazards.Play();
            
            buildingFall?.StartFalling();
            buildingHasFallen = true;
            tntPlaced = false;
            promptE.PromptE_Disable();
            Debug.Log("TNT detonated, building is falling.");
            Destroy(gameObject);
            
            fog.SetActive(true);
        }

        // Show prompt only when entering relevant zones
        if ((playerInRange && !collected && !placedDown) ||
            (playerInZone && collected) ||
            (playerInZone && tntPlaced && readyToDetonate && !buildingHasFallen))
        {
            promptE.PromptE_Show();
        }
    }

    void DropTNTToZone()
    {
        if (tntZoneTransform != null)
        {
            semiTransparent.SetActive(false);
            
            collected = false;
            placedDown = true;
            playerInRange = false;

            transform.position = tntZoneTransform.position + Vector3.up * 0.1f;
            transform.rotation = Quaternion.Euler(0f, -30f, 0f);

            tntPlaced = true;
            readyToDetonate = false;

            StartCoroutine(EnableDetonationNextFrame());

            Debug.Log("TNT placed down.");
        }
        else
        {
            Debug.LogWarning("No TNT zone transform found!");
        }
    }

    IEnumerator EnableDetonationNextFrame()
    {
        yield return null;
        readyToDetonate = true;
        Debug.Log("Detonation now enabled.");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
        else if (other.CompareTag("TNT_Zone"))
        {
            playerInZone = true;
            tntZoneTransform = other.transform;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            promptE.PromptE_Disable();
        }
        else if (other.CompareTag("TNT_Zone"))
        {
            playerInZone = false;
            tntZoneTransform = null;
            promptE.PromptE_Disable();
        }
    }
}