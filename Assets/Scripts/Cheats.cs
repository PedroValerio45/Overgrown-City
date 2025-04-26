using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Cheats : MonoBehaviour
{
    private PlayerData playerData;
    private PlayerInventory playerInventory;
    
    void Start()
    {
        // playerData = GetComponent<PlayerData>();
        playerData = FindObjectOfType<PlayerData>();
        playerInventory = FindObjectOfType<PlayerInventory>();

        if (playerData == null) { Debug.LogError("Cheats: No player data found"); }
        if (playerInventory == null) { Debug.LogError("Cheats: No player inventory found"); }
    }

    void Update()
    {
        Cheater();
    }
    
    private void Cheater()
    {
        // CHEATER CHEATER RAHHHH
        if (Input.GetKeyDown(KeyCode.O))
        {
            playerData.ChangeCurrentHP(1);
            Debug.Log("Cheat: +1 HP. Current HP: " + playerData.playerHP);
        }
        else if (Input.GetKeyDown(KeyCode.I))
        {
            playerData.ChangeCurrentHP(-1);
            Debug.Log("Cheat: -1 HP. Current HP: " + playerData.playerHP);
        }
        else if (Input.GetKeyDown(KeyCode.L))
        {
            PlayerData.isCheating = true;
            playerData.playerHP = 999;
            Debug.Log("Cheat: 999 HP. Current HP: " + playerData.playerHP);
        }
        else if (Input.GetKeyDown(KeyCode.K))
        {
            PlayerData.isCheating = false;
            playerData.playerHP = 1;
            Debug.Log("Cheat: 1 HP. Current HP: " + playerData.playerHP);
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            playerInventory.healthPotionAmount += 1;
            Debug.Log("Cheat: +1 Potions. Current Potion Amount: " + playerInventory.healthPotionAmount);
        }
        else if (Input.GetKeyDown(KeyCode.Keypad0))
        {
            SceneManager.LoadScene(Application.loadedLevel); // Reset scene to test stuff like collectables
        }
    }
}
