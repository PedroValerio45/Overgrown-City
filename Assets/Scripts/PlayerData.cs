using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor.Experimental;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    // STUFF IN THIS SCENE IS TO BE SAVES BETWEEN SCENES

    // this has to be added in all unity scenes that are relevant!
    public UIHealth uiHealth;
    private float playerDamageCooldown = 1f; // Cooldown Amount (Max)
    [SerializeField] private float playerDamageCooldownTimer; // Cooldown Countdown Timer (Min = 0)

    public static int playerMaxHP = 4; // Max HP (fixed value)
    public int playerHP = 4; // Current HP
    public static string playerAura = "3000 gazillions";
    
    // Amount of boat parts collected (will prob go unused)s
    // public static int partsCollectedAmount = 0;
    
    // List of boat parts collected
    public static List<int> partsCollected = new List<int>();
    
    // Cheats ig
    public static bool isCheating = false;

    void Start()
    {
        if (uiHealth == null) { Debug.Log("UI Health null"); }
        // else { Debug.Log("UI Health is literally present in playerData too"); }
        
        // uiHealth = GameObject.Find("UIHealth").GetComponent<UIHealth>();
    }

    void Update()
    {
        if (playerDamageCooldownTimer > 0) { playerDamageCooldownTimer -= Time.deltaTime; }
        else if (playerDamageCooldownTimer < 0) { playerDamageCooldownTimer = 0; }
    }

    public void ChangeCurrentHP(int amount)
    {
        /* if (uiHealth == null)
        {
            Debug.LogError("uiHealth is NOT assigned in PlayerData!");
            return;
        } */
        
        if (playerDamageCooldownTimer <= 0)
        {
            Debug.Log("HP set to: " + playerHP);
            
            playerHP += amount;
            playerDamageCooldownTimer = playerDamageCooldown;
        }
        else
        {
            Debug.Log("Damage blocked by cooldown, timer: " + playerDamageCooldownTimer);
        }

    }

    // PLAYER COLLECTABLES FILE
    public void CreateOrWritePlayerFile_Collectables()
    {
        string filePath = Path.Combine(Application.dataPath, "playerCollectables.txt");

        try
        {
            File.WriteAllLines(filePath, partsCollected.ConvertAll(d => d.ToString()));
            Debug.Log("File saved successfully at: " + filePath);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to save file: " + e.Message);
        }
    }
    
    public List<int> ReadPlayerFile_Collectables()
    {
        // string fileName = "PlayerData.txt";
        string filePath = Path.Combine(Application.dataPath, "playerCollectables.txt");

        if (File.Exists(filePath))
        {
            string[] lines = File.ReadAllLines(filePath);
            
            partsCollected.Clear();

            foreach (string line in lines)
            {
                if (int.TryParse(line, out int number))
                {
                    partsCollected.Add(number);
                }
            }
        }
        else
        {
            print("File does not exist.");
        }

        return partsCollected;
    }
    
    // PLAYER HEALTH FILE
    public void CreateOrWritePlayerFile_Health()
    {
        string filePath = Path.Combine(Application.dataPath, "playerCollectables.txt");

        try
        {
            File.WriteAllLines(filePath, partsCollected.ConvertAll(d => d.ToString()));
            Debug.Log("File saved successfully at: " + filePath);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to save file: " + e.Message);
        }
    }
    
    public List<int> ReadPlayerFile_Health()
    {
        // string fileName = "PlayerData.txt";
        string filePath = Path.Combine(Application.dataPath, "playerCollectables.txt");

        if (File.Exists(filePath))
        {
            string[] lines = File.ReadAllLines(filePath);
            
            partsCollected.Clear();

            foreach (string line in lines)
            {
                if (int.TryParse(line, out int number))
                {
                    partsCollected.Add(number);
                }
            }
        }
        else
        {
            print("File does not exist.");
        }

        return partsCollected;
    }
}