using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable] public class Item
{
    public string itemName;
    public string itemType;
    public int itemID;

    public Item(string name, string type, int id)
    {
        itemName = name;
        itemType = type;
        itemID = id;
    }

    // Item list (HP Potions or whatever are unrelated to the inventory or item list
    // Item list IDs start at 1 (id = 0 is considered null)
    public static Dictionary<int, Item> itemDatabase = new Dictionary<int, Item>()
    {
        { 1, new Item("Gear", "Collectable", 1) },
        { 2, new Item("Propeller", "Collectable", 2) },
        { 3, new Item("Gasoline", "Collectable", 3) },
        
        { 4, new Item("Boat Motor", "BoatPart", 4) }
    };

    public static Item GetItemByID(int id)
    {
        if (itemDatabase.ContainsKey(id))
            return itemDatabase[id];
        else
            return null;
    }
} 

public class PlayerInventory : MonoBehaviour
{
    // EVERYTHING AUDIO HAS TO BE ADDED MANUALLY IN INSPECTOR FOR EVERY INSTANCE OF THIS FILE
    public AudioSource audioSourcePlayerOthers;
    public AudioClip healSound;
    
    public PlayerData playerData;
    public UIHealth uiHealth;
    public UIPotions uiPotions;

    [SerializeField] public int healthPotionAmount;
    public int healthPotionRegenAmount = 2;

    private void Awake()
    {
        playerData = FindObjectOfType<PlayerData>();
        if (playerData == null) { Debug.LogError("Player Inventory: No player data found"); }
    }

    private void Update()
    {
        HealPlayer();
        
        uiPotions.SetCounterText(healthPotionAmount); // Here is the only place the UI element for the hp potions updates
    }

    private void HealPlayer()
    {
        if (Input.GetKeyDown(KeyCode.R) && healthPotionAmount > 0)
        {
            if (playerData.playerHP < PlayerData.playerMaxHP)
            {
                audioSourcePlayerOthers.clip = healSound;
                audioSourcePlayerOthers.Play();
                
                healthPotionAmount--;
                playerData.playerHP += healthPotionAmount;
                if (playerData.playerHP > PlayerData.playerMaxHP && !PlayerData.isCheating)
                {
                    playerData.playerHP = PlayerData.playerMaxHP;
                }
                
                Debug.Log("Healed player, current HP: " + playerData.playerHP);
            }
            else
            {
                Debug.Log("Player HP is full: " + playerData.playerHP);
            }
        }
    }
}