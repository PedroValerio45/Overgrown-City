using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarnivorousPlant : MonoBehaviour
{
    // UI HEALTH HAS TO BE ADDED MANUALLY IN EACH RELEVANT SCENE
    public PlayerData playerData;
    public UIHealth uiHealth;
    public float attackTimerDEBUG;
    public float attackDelayDEBUG = 1f;

    private void Start()
    {
        attackTimerDEBUG = attackDelayDEBUG;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("player IN plant range: " + other.gameObject.name);
    }
    
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CharacterController playerCC = other.GetComponent<CharacterController>();
            if (attackTimerDEBUG > 0)
            {
                attackTimerDEBUG -= Time.deltaTime;
            }
            else
            {
                Vector3 knockbackDirection = (other.transform.position - transform.position).normalized;
                playerCC.Move(knockbackDirection * 3f);

                playerData.ChangeCurrentHP(-1);
            
                Debug.Log("CarnivorousPlant attacked player, Current HP: " + PlayerStats.playerHP);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            attackTimerDEBUG = attackDelayDEBUG;
            Debug.Log("player OUT of plant range: " + other.gameObject.name);
        }
    }
}
