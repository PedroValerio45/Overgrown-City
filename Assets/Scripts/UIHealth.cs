using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHealth : MonoBehaviour
{
    // EVERYTHING HERE IS TO CHANGE THE HP UI GRAPHIC
    public Slider healthSlider;

    /* void Start()
    {
        healthSlider = GetComponentInChildren<Slider>();
        healthSlider = GameObject.Find("HealthBar").GetComponent<Slider>();

        if (healthSlider == null) { Debug.Log("why is this so fucking difficult"); }
        else { Debug.Log("health slider present in ui health"); }
    } */
    
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