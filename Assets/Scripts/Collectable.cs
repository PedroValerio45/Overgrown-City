using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    public GameObject player;
    public PlayerData playerData;
    public PlayerInventory playerInventory;
    
    // THE COLLECTABLE ID CANNOT BE ZERO
    [SerializeField] public int collectableID;
    bool collected;
    bool playerInRange;
    
    private void Awake()
    {
        playerData = player.GetComponent<PlayerData>();
        playerInventory = player.GetComponent<PlayerInventory>();

        PlayerData.partsCollected = playerData.ReadPlayerFile_Collectables();
        if (PlayerData.partsCollected.Contains(collectableID)) { collected = true; }
        
        switch (collectableID)
        {
            case 1:
                GetComponent<Renderer>().material.color = Color.yellow;
                // Debug.Log($"Set color for ID {collectableID}");
                break;
            case 2:
                GetComponent<Renderer>().material.color = Color.red;
                // Debug.Log($"Set color for ID {collectableID}");
                break;
            case 3:
                GetComponent<Renderer>().material.color = Color.blue;
                // Debug.Log($"Set color for ID {collectableID}");
                break;
            
            // Just for testing, we don't have a 4th quest item
            case 4:
                GetComponent<Renderer>().material.color = Color.magenta;
                // Debug.Log($"Set color for ID {collectableID}");
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
            
            playerInventory.AddItem(Item.GetItemByID(collectableID));
            
            Debug.Log($"AFTER ADDING - inventory count: {playerInventory.playerInv.Count}");
            
            playerData.CreateOrWritePlayerFile_Collectables();
        }
        
        if (collected) { gameObject.SetActive(false); }
        
        // Debug.Log($"Current inventory count: {playerInventory.playerInv.Count}");
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