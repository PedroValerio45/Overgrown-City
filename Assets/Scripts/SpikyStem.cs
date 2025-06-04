using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikyStem : MonoBehaviour
{
    // EVERYTHING AUDIO HAS TO BE ADDED MANUALLY IN INSPECTOR FOR EVERY INSTANCE OF THIS FILE
    public AudioSource audioSourcePlayerOthers;
    public AudioClip stemDamageSound;
    
    public PlayerData playerData;
    
    private CharacterController cc;
    private Vector3 knockbackVelocity;
    private float knockbackDuration = 0.25f;
    [SerializeField] private float knockbackTimer;

    void Start()
    {
        // playerData = GetComponent<PlayerData>();
        playerData = FindObjectOfType<PlayerData>();
        cc = FindObjectOfType<CharacterController>();
        
        if (playerData == null) { Debug.LogError("Spiky Stem: No player data found"); }
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
    
    /* private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CharacterController playerCC = other.GetComponent<CharacterController>();
            Vector3 knockbackDirection = (other.transform.position - transform.position).normalized;
            playerCC.Move(knockbackDirection * 2f);

            playerData.ChangeCurrentHP(-1);
        
            Debug.Log("player collided with stem, Current HP: " + playerData.playerHP);
        }
    } */
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (knockbackTimer <= 0)
            {
                Vector3 direction = (other.transform.position - transform.position).normalized;
                ApplyKnockback(direction, 50f, 0.25f);
            }
            
            audioSourcePlayerOthers.pitch = Random.Range(0.8f, 1.2f);
            audioSourcePlayerOthers.clip = stemDamageSound;
            audioSourcePlayerOthers.Play();
            
            playerData.ChangeCurrentHP(-1);
            
            Debug.Log("player collided with stem, Current HP: " + playerData.playerHP);
        }
    }
    
    public void ApplyKnockback(Vector3 direction, float force, float duration)
    {
        knockbackVelocity = direction.normalized * force;
        knockbackDuration = duration;
        knockbackTimer = duration;
    }

}
