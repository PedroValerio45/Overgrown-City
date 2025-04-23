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
    
    private CharacterController cc;
    private Vector3 knockbackVelocity;
    private float knockbackDuration = 0.25f;
    [SerializeField] private float knockbackTimer;

    private void Start()
    {
        attackTimerDEBUG = attackDelayDEBUG;
        
        // playerData = GetComponent<PlayerData>();
        playerData = FindObjectOfType<PlayerData>();
        cc = FindObjectOfType<CharacterController>();
        
        if (playerData == null) { Debug.LogError("Carnivorous Plant: No player data found"); }
        if (cc == null) { Debug.LogError("Spiky Stem: No cc found"); }
    }
    
    void Update()
    {
        if (knockbackTimer > 0)
        {
            float t = knockbackTimer / knockbackDuration;
            float easedT = Mathf.SmoothStep(0, 1, t); // 0 = no force, 1 = full force
            Vector3 easedVelocity = knockbackVelocity * easedT;
            cc.Move(easedVelocity * Time.deltaTime);

            knockbackTimer -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("player IN plant range: " + other.gameObject.name);
    }
    
    /* private void OnTriggerStay(Collider other)
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
            
                Debug.Log("CarnivorousPlant attacked player, Current HP: " + playerData.playerHP);
            }
        }
    } */
    
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
                if (knockbackTimer <= 0)
                {
                    Vector3 direction = (other.transform.position - transform.position).normalized;
                    ApplyKnockback(direction, 50f, 0.25f);
                }

                playerData.ChangeCurrentHP(-1);
            
                Debug.Log("CarnivorousPlant attacked player, Current HP: " + playerData.playerHP);
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
    
    public void ApplyKnockback(Vector3 direction, float force, float duration)
    {
        knockbackVelocity = direction.normalized * force;
        knockbackDuration = duration;
        knockbackTimer = duration;
    }
}
