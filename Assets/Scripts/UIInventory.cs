using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIInventory : MonoBehaviour
{
    public PlayerData playerData;
    public PlayerInventory playerInventory;
    public GameObject UIInventoryImage;

    public float positionXOpen;
    public float positionXClosed = 1180f;
    
    private bool menuOpen;
    [SerializeField] private float menuTimer;
    private float menuMaxTimer = 2f;
    
    void Start()
    {
        playerData = FindObjectOfType<PlayerData>();
        playerInventory = FindObjectOfType<PlayerInventory>();

        if (playerData == null) { Debug.LogError("Cheats: No player data found"); }
        if (playerInventory == null) { Debug.LogError("Cheats: No player inventory found"); }

        positionXOpen = UIInventoryImage.transform.position.x;
    }

    void Update()
    {
        if (menuTimer < menuMaxTimer)
        {
            menuTimer += Time.deltaTime;
        }
        
        if (Input.GetKeyDown(KeyCode.Tab) && menuTimer >= menuMaxTimer)
        {
            menuTimer = 0f;
            
            if (menuOpen)
            {
                menuOpen = false;
                Vector3.Lerp(
                    new Vector3(positionXOpen, UIInventoryImage.transform.position.y, UIInventoryImage.transform.position.z),
                    new Vector3(positionXClosed, UIInventoryImage.transform.position.y, UIInventoryImage.transform.position.z),
                    Time.deltaTime
                );
            }
            else
            {
                menuOpen = true;
                Vector3.Lerp(
                    new Vector3(positionXClosed, UIInventoryImage.transform.position.y, UIInventoryImage.transform.position.z),
                    new Vector3(positionXOpen, UIInventoryImage.transform.position.y, UIInventoryImage.transform.position.z),
                    Time.deltaTime
                );
            }
            
            Debug.Log("UI coiso should move NOW");
        }
    }
}