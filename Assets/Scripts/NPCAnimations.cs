using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAnimations : MonoBehaviour
{
    public NPC_Behaviour NpcBehaviour;
    public Animator NPCAnimator;
    
    [SerializeField] private bool isWaiting;
    [SerializeField] private bool isTalking;
    
    void Start()
    {
        
    }

    void Update()
    {
        isWaiting = NpcBehaviour.isWaiting;
        isTalking = NpcBehaviour.isFrozen;
        
        NPCAnimator.SetBool("isWalking", !isWaiting);
        NPCAnimator.SetBool("isTalking", isTalking);
    }
}
