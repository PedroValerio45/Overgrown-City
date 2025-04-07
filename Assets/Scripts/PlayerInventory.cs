using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public int inventorySize = 10;
    public static List<Item> playerInv = new List<Item>();

    private void Start()
    {
        // Initialize empty slots
        for (int i = 0; i < inventorySize; i++)
        {
            playerInv.Add(null);
        }
    }

    public bool AddItem(Item newItem)
    {
        for (int i = 0; i < playerInv.Count; i++)
        {
            if (playerInv[i] == null)
            {
                playerInv[i] = newItem;
                Debug.Log($"Added {newItem.itemName} to slot {i}");
                LogInventory();
                return true;
            }
        }

        Debug.Log("Inventory full!");
        LogInventory();
        return false;
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

public class Item
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
    
    // ITEM LIST
    public static Dictionary<int, Item> itemDatabase = new Dictionary<int, Item>()
    {
        { 0, new Item("Health Potion", "Nurse", 0) },
        
        { 1, new Item("collectable0", "Collectable", 1) },
        { 2, new Item("collectable1", "Collectable", 2) },
        { 3, new Item("collectable2", "Collectable", 3) },
        
        { 4, new Item("boatPart1", "BoatPart", 4) }
    };

    public static Item GetItemByID(int id)
    {
        if (itemDatabase.ContainsKey(id))
            return itemDatabase[id];
        else
            return null;
    }
}