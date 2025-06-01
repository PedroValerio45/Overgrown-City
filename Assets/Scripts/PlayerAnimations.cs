using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    public PlayerData playerData;
    public ThirdPersonMovement thirdPersonMovement;
    // public ClimbObject climbObject;
    public Animator playerAnimator;
    
    [SerializeField] private bool isWalking;
    [SerializeField] private bool isJumping;
    [SerializeField] private bool isClimbing;
    [SerializeField] private bool playerWasHit;
    [SerializeField] private float climbingSpeedAnimMultiplier;
    
    void Start()
    {
        playerData = FindObjectOfType<PlayerData>();
        // climbObject = FindObjectOfType<ClimbObject>();
        thirdPersonMovement = FindObjectOfType<ThirdPersonMovement>();
    }

    void Update()
    {
        isWalking = thirdPersonMovement.isWalking; // Done?
        isJumping =  thirdPersonMovement.isJumping; // Done?
        isClimbing = thirdPersonMovement.isClimbing; // Done?
        climbingSpeedAnimMultiplier =  thirdPersonMovement.climbingSpeedAnimMultiplier; // Done?
        
        playerWasHit = playerData.playerWasHit; // Done?
        
        playerAnimator.SetBool("isWalking", isWalking);
        playerAnimator.SetBool("isJumping", isJumping);
        playerAnimator.SetBool("isClimbing", isClimbing);
        playerAnimator.SetBool("isDamaged", playerWasHit);
        playerAnimator.SetFloat("climbSpeed", climbingSpeedAnimMultiplier);
    }
}
