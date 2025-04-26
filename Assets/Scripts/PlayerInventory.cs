using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public PlayerData playerData;
    public UIHealth uiHealth;
    public UIPotions uiPotions;
    
    // public int inventorySize = 3;
    public List<Item> playerInv = new List<Item>();

    [SerializeField] public int healthPotionAmount;
    public int healthPotionRegenAmount = 2;

    private void Awake()
    {
        playerData = FindObjectOfType<PlayerData>();
        if (playerData == null) { Debug.LogError("Player Inventory: No player data found"); }
        
        // Initialize empty slots
        // for (int i = 0; i < inventorySize; i++) { playerInv.Add(null); }

        // Fill the inventory based on partsCollected
        foreach (int partID in PlayerData.partsCollected)
        {
            Item item = Item.GetItemByID(partID);
            if (item != null)
            {
                AddItem(item);
            }
        }
    }

    private void Update()
    {
        HealPlayer();

        // LogInventory();
        
        uiPotions.SetCounterText(healthPotionAmount); // Here is the only place the UI element for the hp potions updates
    }

    private void HealPlayer()
    {
        if (Input.GetKeyDown(KeyCode.R) && healthPotionAmount > 0)
        {
            if (playerData.playerHP < PlayerData.playerMaxHP)
            {
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

    public bool AddItem(Item newItem)
    {
        Debug.Log($"Attempting to add item: {newItem.itemName}. Current inventory count: {playerInv.Count}");
        
        /* for (int i = 0; i < playerInv.Count; i++)
        {
            if (playerInv[i] == null)
            {
                playerInv[i] = newItem;
                Debug.Log($"Added {newItem.itemName} to slot {i}");
                LogInventory();
                return true;
            }
        }*/ 
        
        Debug.Log($"Before adding: {playerInv.Count}");
        playerInv.Add(newItem);
        Debug.Log($"Added {newItem.itemName}");
        Debug.Log($"After adding: {playerInv.Count}");
        
        return true;
        
        /* Debug.Log("Current Inventory Count (Add failure): " +  playerInv.Count);
        Debug.Log("Inventory full!");
        LogInventory();
        return false; */
    }

    public void RemoveItem(int slotIndex)
    {
        if (slotIndex >= 0 && slotIndex < playerInv.Count && playerInv[slotIndex] != null)
        {
            Debug.Log($"Removed {playerInv[slotIndex].itemName} from slot {slotIndex}");
            playerInv[slotIndex] = null;
            LogInventory();
        }
    }
    
    public void LogInventory()
    {
        Debug.Log("=== Inventory ===");

        for (int i = 0; i < playerInv.Count; i++)
        {
            if (playerInv[i] != null)
            {
                Item item = playerInv[i];
                Debug.Log($"Slot {i}: {item.itemName} (Type: {item.itemType}, ID: {item.itemID})");
            }
            else
            {
                Debug.Log($"Slot {i}: [Empty]");
            }
        }
        
        Debug.Log("=================");
    }
}

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

    // ITEM LIST (HP Potions or whatever are unrelated to the inventory or item list
    // Item list IDs start at 1 (id = 0 is considered null ig)
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