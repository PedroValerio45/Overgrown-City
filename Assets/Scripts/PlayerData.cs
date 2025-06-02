using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor.Experimental;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    // STUFF IN THIS SCENE IS TO BE SAVES BETWEEN SCENES

    public UIHealth uiHealth;
    private float playerDamageCooldown = 1f; // Cooldown Amount (Max)
    [SerializeField] private float playerDamageCooldownTimer; // Cooldown Countdown Timer (Min = 0)

    public static int playerMaxHP = 4; // Max HP (fixed value)
    public int playerHP = 4; // Current HP
    public static string playerAura = "3000 gazillions";

    private float lastYPosition;
    private float fallThreshold = 10f;
    private float fallDamageMultiplier = 0.15f;
    private bool isFalling;

    public static bool hasBegunQuest = false;
    public static bool hasFinishedQuest = false;
    
    // Needed for animations
    [SerializeField] public bool playerWasHit;
    private float damageAnimTimer = 0.5f; // Timer for the thing below
    private float damageAnimTimerMax = 0.5f; // How long to allow the damage animation to play

    // Amount of quest items collected
    [SerializeField] public static int amountQuestItemsCollected;
    
    // List of boat parts collected
    [SerializeField] public static List<int> partsCollected = new List<int>();
    
    // Cheats ig
    public static bool isCheating = false;

    void Start()
    {
        if (uiHealth == null) { Debug.Log("UI Health null"); }
        uiHealth = FindObjectOfType<UIHealth>();
        
        // uiHealth = GameObject.Find("UIHealth").GetComponent<UIHealth>();
        
        Screen.SetResolution(1920, 1080, false);
    }

    void Update()
    {
        if (playerDamageCooldownTimer > 0) { playerDamageCooldownTimer -= Time.deltaTime; }
        else if (playerDamageCooldownTimer < 0) { playerDamageCooldownTimer = 0; }

        CharacterController controller = GetComponent<CharacterController>();

        if (!controller.isGrounded)
        {
            if (!isFalling)
            {
                isFalling = true;
                lastYPosition = transform.position.y;
            }
        }
        else
        {
            if (isFalling)
            {
                float fallDistance = lastYPosition - transform.position.y;

                if (fallDistance > fallThreshold)
                {
                    int damage = Mathf.RoundToInt((fallDistance - fallThreshold) * fallDamageMultiplier);
                    Debug.Log($"Fall distance: {fallDistance-fallThreshold}, Damage: {damage}");
                    ChangeCurrentHP(-damage);
                }

                isFalling = false;
            }
        }

        if (damageAnimTimer < damageAnimTimerMax)
        {
            damageAnimTimer += Time.deltaTime;
            playerWasHit  = true;
        }
        else
        {
            playerWasHit =  false;
        }
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
            playerHP += amount;

            Debug.Log("HP set to: " + playerHP);

            playerHP = Mathf.Clamp(playerHP, 0, playerMaxHP);

            playerDamageCooldownTimer = playerDamageCooldown;

            damageAnimTimer = 0f;
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
        string filePath = Path.Combine(Application.dataPath, "playerCollectables.txt");

        amountQuestItemsCollected = 0; // <-- RESET the counter

        if (File.Exists(filePath))
        {
            string[] lines = File.ReadAllLines(filePath);

            partsCollected.Clear();

            foreach (string line in lines)
            {
                if (int.TryParse(line, out int number))
                {
                    partsCollected.Add(number);
                    if (number <= 3)
                    {
                        amountQuestItemsCollected++;
                    }
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

    public void CollectPart(int partID)
    {
        if (!partsCollected.Contains(partID))
        {
            partsCollected.Add(partID);

            if (partID <= 3)
            {
                amountQuestItemsCollected += 1;
            }

            Debug.Log($"Collected item with ID: {partID}. Total qualifying parts: {amountQuestItemsCollected}");
        }
        else
        {
            Debug.Log($"Item ID {partID} already collected.");
        }

        LogPartsCollected();
    }

    public void RemoveItem(int partID)
    {
        if (partsCollected.Contains(partID))
        {
            partsCollected.Remove(partID);

            Debug.Log($"Removed item with ID: {partID}.");
        }
        else
        {
            Debug.Log($"I guess you can say item with ID {partID} is a HYPER GONER.");
        }

        LogPartsCollected();
    }

    public void LogPartsCollected()
    {
        Debug.Log("=== Parts Collected ===");
        if (partsCollected.Count == 0)
        {
            Debug.Log("No parts collected.");
        }
        else
        {
            foreach (int partID in partsCollected)
            {
                Debug.Log($"Part ID: {partID}");
            }
        }
        Debug.Log("=======================");
    }
}