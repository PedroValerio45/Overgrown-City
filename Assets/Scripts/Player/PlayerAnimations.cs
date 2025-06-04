using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    public PlayerData playerData;
    public ThirdPersonMovement thirdPersonMovement;
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
    }

    void Update()
    {
        if (playerData.playerHP > 0)
        {
            isWalking = thirdPersonMovement.isWalking;
            jumpingAnimTimer = thirdPersonMovement.jumpingAnimTimer;
            jumpingAnimTimerMax = thirdPersonMovement.jumpingAnimTimerMax;
            isClimbing = thirdPersonMovement.isClimbing;
            climbingSpeedAnimMultiplier = thirdPersonMovement.climbingSpeedAnimMultiplier;
            playerWasHit = playerData.playerWasHit;
        }
        else
        {
            playerWasHit = true;
        }

        playerAnimator.SetBool("isWalking", isWalking);
        playerAnimator.SetBool("isJumping", isJumping);
        playerAnimator.SetBool("isDamaged", playerWasHit);
        playerClimbAnimator.SetFloat("climbSpeedMultiplier", climbingSpeedAnimMultiplier);

        if (isClimbing)
        {
            regularModel.SetActive(false);
            climbingModel.SetActive(true);

            GameObject nearestClimbable = GetNearestClimbable();
            if (nearestClimbable != null)
            {
                Vector3 direction = nearestClimbable.transform.position - transform.position;
                direction.y = 0; // Ignore vertical difference between player and ladder

                if (direction != Vector3.zero)
                {
                    transform.rotation = Quaternion.LookRotation(direction);
                }
            }
        }
        else
        {
            regularModel.SetActive(true);
            climbingModel.SetActive(false);
        }

        if (jumpingAnimTimer < jumpingAnimTimerMax)
        {
            isJumping = true;
        }
        else
        {
            isJumping = false;
        }
    }

    private GameObject GetNearestClimbable()
    {
        GameObject[] climbables = GameObject.FindGameObjectsWithTag("Climb");
        GameObject nearest = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = transform.position;

        foreach (GameObject obj in climbables)
        {
            float dist = Vector3.Distance(currentPos, obj.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                nearest = obj;
            }
        }

        return nearest;
    }
}