using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAnimations : MonoBehaviour
{
    public NPC_Behaviour NpcBehaviour;
    public Animator NPCAnimator;
    
    [SerializeField] private bool isWaiting;
    [SerializeField] private bool isTalking;
    [SerializeField] private bool isExtraAction;
    
    void Update()
    {
        isWaiting = NpcBehaviour.currentState == NPCState.Waiting;
        isTalking = NpcBehaviour.currentState == NPCState.Talking;
        isExtraAction = NpcBehaviour.currentState == NPCState.ExtraAction;

        NPCAnimator.SetBool("isWalking", !isWaiting);
        NPCAnimator.SetBool("isTalking", isTalking);
        NPCAnimator.SetBool("isExtraAction", isExtraAction);
    }
}
