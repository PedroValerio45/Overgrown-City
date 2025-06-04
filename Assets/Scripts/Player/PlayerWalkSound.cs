using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalkSound : MonoBehaviour
{
    // EVERYTHING AUDIO HAS TO BE ADDED MANUALLY IN INSPECTOR FOR EVERY INSTANCE OF THIS FILE
    public AudioSource audioSourcePlayerWalk;
    public AudioClip grassWalkSound;
    
    public void playWalkSound()
    {
        audioSourcePlayerWalk.pitch = Random.Range(0.8f, 1.2f);
        audioSourcePlayerWalk.clip = grassWalkSound;
        audioSourcePlayerWalk.Play();
    }
}
