using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInventory : MonoBehaviour
{
    public PlayerData playerData;
    public PlayerInventory playerInventory;
    public GameObject UIInventoryImage;
    public Image itemImage0;
    public Image itemImage1;
    public Image itemImage2;

    private float positionXOpen;
    private float positionXClosed;

    private bool menuOpen;
    private bool isAnimating;
    private float animationTimer;
    private float animationDuration = 0.25f;
    private Vector3 animationStart;
    private Vector3 animationEnd;
    
    public Sprite ItemSpriteEmpty;
    public Sprite itemSprite1;
    public Sprite itemSprite2;
    public Sprite itemSprite3;
    public GameObject bagImage;

    void Start()
    {
        playerData = FindObjectOfType<PlayerData>();
        playerInventory = FindObjectOfType<PlayerInventory>();

        if (playerData == null) Debug.LogError("No player data found");
        if (playerInventory == null) Debug.LogError("No player inventory found");

        positionXOpen = UIInventoryImage.transform.position.x;
        positionXClosed = positionXOpen + 430f;

        menuOpen = false;
        UIInventoryImage.transform.position = new Vector3(
            positionXClosed,
            UIInventoryImage.transform.position.y,
            UIInventoryImage.transform.position.z
        );
        
        SetItemInvSlotImage(0);
        SetItemInvSlotImage(1);
        SetItemInvSlotImage(2);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && !isAnimating)
        {
            animationTimer = 0f;
            isAnimating = true;

            animationStart = UIInventoryImage.transform.position;
            if (menuOpen)
            {
                animationEnd = new Vector3(positionXClosed, animationStart.y, animationStart.z);
            }
            else
            {
                animationEnd = new Vector3(positionXOpen, animationStart.y, animationStart.z);
            }

            menuOpen = !menuOpen;
        }

        if (isAnimating)
        {
            animationTimer += Time.deltaTime;
            float t = Mathf.Clamp01(animationTimer / animationDuration);

            UIInventoryImage.transform.position = Vector3.Lerp(animationStart, animationEnd, t);

            if (t >= 1f)
            {
                isAnimating = false;
            }
        }

        if (!menuOpen && !isAnimating) { bagImage.SetActive(true); }
        else { bagImage.SetActive(false); }
    }

    public void SetItemInvSlotImage(int image)
    {
        Image itemImage;
        
        switch (image)
        {
            case 0:
                itemImage = itemImage0;
                break;
            case 1:
                itemImage = itemImage1;
                break;
            case 2:
                itemImage = itemImage2;
                break;
            default:
                itemImage = itemImage0;
                Debug.LogError("SOME MORON IS USING IMAGE > 2 IN UI INVENTORY");
                break;
        }
        
        switch (SetItems(image))
        {
            case 1:
                itemImage.sprite = itemSprite1;
                Debug.Log("Set Items returned: 1");
                break;
            case 2:
                itemImage.sprite = itemSprite2;
                Debug.Log("Set Items returned: 2");
                break;
            case 3:
                itemImage.sprite = itemSprite3;
                Debug.Log("Set Items returned: 3");
                break;
            default:
                itemImage.sprite = ItemSpriteEmpty;
                Debug.Log("Set Items returned empty or -1");
                break;
        }
    }
    
    public int SetItems(int slot)
    {
        if (playerInventory == null)
        {
            Debug.LogError("Player Inventory is null");
            return -1;
        }

        if (PlayerData.partsCollected == null)
        {
            Debug.LogError("Player Inventory list is null");
            return -1;
        }

        if (slot < 0 || slot >= PlayerData.partsCollected.Count)
        {
            Debug.LogError($"Slot index {slot} is out of bounds");
            return -1;
        }

        if (PlayerData.partsCollected[slot] == 0)
        {
            Debug.Log($"Slot {slot} is empty");
            return -1;
        }

        // If we reach here, we have a valid item
        Debug.Log($"Item found in slot {slot}: {PlayerData.partsCollected[slot]} (ID: {PlayerData.partsCollected[slot]})");
    
        return PlayerData.partsCollected[slot];
    }
}