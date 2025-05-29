using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    // public GameObject player;
    public PlayerData playerData;
    // public PlayerInventory playerInventory;
    public UIInventory uiInventory;
    public promptE promptE;
    
    public GameObject gasoline;
    public GameObject gears;
    public GameObject propeller;
    
    // THE COLLECTABLE ID CANNOT BE ZERO
    [SerializeField] public int collectableID;
    bool collected;
    bool playerInRange;
    
    private void Awake()
    {
        playerData = FindObjectOfType<PlayerData>();
        // playerInventory = FindObjectOfType<PlayerInventory>();
        uiInventory  = FindObjectOfType<UIInventory>();
        promptE = FindObjectOfType<promptE>();

        PlayerData.partsCollected = playerData.ReadPlayerFile_Collectables();
        if (PlayerData.partsCollected.Contains(collectableID)) { collected = true; }
        
        GetComponent<Renderer>().enabled = false;
        
        switch (collectableID)
        {
            case 1:
                gasoline.SetActive(true);
                // GetComponent<Renderer>().material.color = Color.yellow;
                // Debug.Log($"Set color for ID {collectableID}");
                break;
            case 2:
                gears.SetActive(true);
                // GetComponent<Renderer>().material.color = Color.red;
                // Debug.Log($"Set color for ID {collectableID}");
                break;
            case 3:
                propeller.SetActive(true);
                // GetComponent<Renderer>().material.color = Color.blue;
                // Debug.Log($"Set color for ID {collectableID}");
                break;
        }
        
        // Just for testing
        if (collectableID < 1 || collectableID > 3)
        {
            GetComponent<Renderer>().enabled = true;
            GetComponent<Renderer>().material.color = Color.magenta;
            // Debug.Log($"Set color for ID {collectableID}");
        }
    }

    void Update()
    {
        if (playerInRange && !collected && Input.GetKeyDown(KeyCode.E))
        {
            collected = true;
            // PlayerData.partsCollectedAmount += 1;
            //PlayerData.partsCollected.Add(collectableID);
                
            // playerInventory.AddItem(Item.GetItemByID(collectableID));
            playerData.CollectPart(collectableID);
            // Debug.Log($"AFTER ADDING - inventory count: {playerInventory.playerInv.Count}");
            
            playerData.CreateOrWritePlayerFile_Collectables();
            
            uiInventory.SetItemInvSlotImage(0);
            uiInventory.SetItemInvSlotImage(1);
            uiInventory.SetItemInvSlotImage(2);
        }

        if (collected)
        {
            gameObject.SetActive(false);
            promptE.PromptE_Disable();
        }
        
        // Debug.Log($"Current inventory count: {playerInventory.playerInv.Count}");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            promptE.PromptE_Show();
            Debug.Log("Player IN range of collectable" + collectableID);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            promptE.PromptE_Disable();
            Debug.Log("Player OUT of range of collectable" + collectableID);
        }
    }
}