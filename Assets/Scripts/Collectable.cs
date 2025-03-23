using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    public PlayerData playerData;
    
    public int collectableID;
    bool collected = false;
    bool playerInRange = false;

    private void Start()
    {
        playerData = GetComponent<PlayerData>();

        PlayerData.partsCollected = playerData.ReadPlayerFile();
        if (PlayerData.partsCollected.Contains(collectableID)) { collected = true; }
        
        switch (collectableID)
        {
            case 0:
                GetComponent<Renderer>().material.color = Color.yellow;
                break;
            case 1:
                GetComponent<Renderer>().material.color = Color.red;
                break;
            case 2:
                GetComponent<Renderer>().material.color = Color.blue;
                break;
            case 3:
                GetComponent<Renderer>().material.color = Color.magenta;
                break;
        }
    }

    void Update()
    {
        if (playerInRange && !collected && Input.GetKeyDown(KeyCode.E))
        {
            collected = true;
            // PlayerData.partsCollectedAmount += 1;
            PlayerData.partsCollected.Add(collectableID);
            playerData.CreateOrWritePlayerFile();
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