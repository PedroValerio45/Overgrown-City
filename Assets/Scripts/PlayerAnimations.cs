using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    public PlayerData playerData;
    public ThirdPersonMovement thirdPersonMovement;
    // public ClimbObject climbObject;
    public Animator playerAnimator;
    public Animator playerClimbAnimator;
    public GameObject regularModel;
    public GameObject climbingModel;
    
    [SerializeField] private bool isWalking;
    [SerializeField] private bool isJumping;
    [SerializeField] private bool isClimbing;
    [SerializeField] private bool playerWasHit;
    [SerializeField] private float climbingSpeedAnimMultiplier;
    [SerializeField] private float jumpingAnimTimer;
    private float jumpingAnimTimerMax;
    
    void Start()
    {
        playerData = FindObjectOfType<PlayerData>();
        thirdPersonMovement = FindObjectOfType<ThirdPersonMovement>();
        // climbObject = FindObjectOfType<ClimbObject>();
    }

    void Update()
    {
        isWalking = thirdPersonMovement.isWalking; // perfect?
        jumpingAnimTimer = thirdPersonMovement.jumpingAnimTimer; // animation freezes sometimes and can start playing mid air
        jumpingAnimTimerMax = thirdPersonMovement.jumpingAnimTimerMax; // perfect
        isClimbing = thirdPersonMovement.isClimbing; // using different model, perfect ig
        climbingSpeedAnimMultiplier =  thirdPersonMovement.climbingSpeedAnimMultiplier; // perfect ig
        // isJumping =  thirdPersonMovement.isJumping;
        
        playerWasHit = playerData.playerWasHit; // perfect
        
        playerAnimator.SetBool("isWalking", isWalking);
        playerAnimator.SetBool("isJumping", isJumping);
        // playerAnimator.SetBool("isClimbing", isClimbing);
        playerAnimator.SetBool("isDamaged", playerWasHit);
        playerClimbAnimator.SetFloat("climbSpeedMultiplier", climbingSpeedAnimMultiplier);

        if (isClimbing)
        {
            regularModel.SetActive(false);
            climbingModel.SetActive(true);
        }
        else
        {
            regularModel.SetActive(true);
            climbingModel.SetActive(false);
        }

        if (jumpingAnimTimer < jumpingAnimTimerMax)
        {
            isJumping =  true;
        }
        else
        {
            isJumping = false;
        }
    }
}
