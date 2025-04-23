using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHealth : MonoBehaviour
{
    // EVERYTHING IN THIS SCRPIT IS TO CHANGE THE HP UI GRAPHIC
    public Slider healthSlider;
    public PlayerData playerData;

    void Start()
    {
        /* healthSlider = GetComponentInChildren<Slider>();
        healthSlider = GameObject.Find("HealthBar").GetComponent<Slider>();

        if (healthSlider == null) { Debug.Log("why is this so fucking difficult"); }
        else { Debug.Log("health slider present in ui health"); } */
        
        // playerData = GetComponent<PlayerData>();
        playerData = FindObjectOfType<PlayerData>();
        if (playerData == null) { Debug.LogError("UI Health: No player data found"); }
        
        SetMaxHealth(PlayerData.playerMaxHP);
        // SetHealth(playerData.playerHP);
        
        Debug.Log("Current player HP: " + PlayerData.playerMaxHP);
    }

    void Update()
    {
        SetHealth(playerData.playerHP); // here is the only place the UI element for the HP updates
    }
    
    public void SetMaxHealth(int health)
    {
        healthSlider.maxValue = health;
        healthSlider.value = health;
    }

    public void SetHealth(int health)
    {
        healthSlider.value = health;
    }
}