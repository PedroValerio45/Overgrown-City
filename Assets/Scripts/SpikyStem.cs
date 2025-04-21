using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikyStem : MonoBehaviour
{
    // UI HEALTH HAS TO BE ADDED MANUALLY IN EACH RELEVANT SCENE
    public UIHealth uiHealth;
    public PlayerData playerData;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CharacterController playerCC = other.GetComponent<CharacterController>();
            Vector3 knockbackDirection = (other.transform.position - transform.position).normalized;
            playerCC.Move(knockbackDirection * 2f);

            playerData.ChangeCurrentHP(-1);
        
            Debug.Log("player collided with stem, Current HP: " + PlayerStats.playerHP);
        }
    }
}
