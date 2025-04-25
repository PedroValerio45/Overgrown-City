using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    public GameObject player;
    public PlayerData playerData;
    public PlayerInventory playerInv;
    
    [SerializeField] public int collectableID;
    bool collected = false;
    bool playerInRange = false;
    
    private void Awake()
    {
        playerData = player.GetComponent<PlayerData>();
        playerInv = player.GetComponent<PlayerInventory>();

        PlayerData.partsCollected = playerData.ReadPlayerFile_Collectables();
        if (PlayerData.partsCollected.Contains(collectableID)) { collected = true; }
        
        switch (collectableID)
        {
            case 0:
                GetComponent<Renderer>().material.color = Color.yellow;
                Debug.Log($"Set color for ID {collectableID}");
                break;
            case 1:
                GetComponent<Renderer>().material.color = Color.red;
                Debug.Log($"Set color for ID {collectableID}");
                break;
            case 2:
                GetComponent<Renderer>().material.color = Color.blue;
                Debug.Log($"Set color for ID {collectableID}");
                break;
            case 3:
                GetComponent<Renderer>().material.color = Color.magenta;
                Debug.Log($"Set color for ID {collectableID}");
                break;
        }
    }

    void Update()
    {
        if (playerInRange && !collected && Input.GetKeyDown(KeyCode.E))
        {
            
            collected = true;
            // PlayerData.partsCollectedAmount += 1;
            //PlayerData.partsCollected.Add(collectableID);
            playerData.CollectPart(collectableID);
            playerInv.AddItem(Item.GetItemByID(collectableID + 1)); // because 0 is the hp potion thing
            playerData.CreateOrWritePlayerFile_Collectables();
            // Debug.Log("Collected amount: " + PlayerData.partsCollectedAmount + ". List: " + PlayerData.partsCollected);
            Debug.Log("Collected List: " + PlayerData.partsCollected);
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