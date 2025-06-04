using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepYourselfSafe : MonoBehaviour
{
    public PlayerData playerData;
    
    private void Start()
    {
        playerData = FindObjectOfType<PlayerData>();
        if (playerData == null) { Debug.LogError("KYS: No player data found"); }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerData.ChangeCurrentHP(-999);
            Debug.Log("Player fell");
        }
    }
}