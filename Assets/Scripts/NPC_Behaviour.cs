using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Finite State Machine states
public enum NPCState
{
    Idle,
    Moving,
    Waiting,
    Talking,
    ExtraAction
}

public class NPC_Behaviour : MonoBehaviour
{
    [SerializeField] public CapsuleCollider cc;
    [SerializeField] private Rigidbody rb;
    public promptE promptE;

    // A* pathfinding stuff
    [SerializeField] public Waypoint currentWaypoint;
    [SerializeField] public float moveSpeed;
    [SerializeField] public bool isWaiting = false; // Needed for animations
    private float originalSpeed;
    private List<Waypoint> path;
    private int currentPathIndex = 0;

    // Dialogue stuff
    [SerializeField] public float interactionRadius;
    [SerializeField] public int npcID;
    [SerializeField] public string npcName;
    [SerializeField] public bool hasInteractedWithPlayer;
    [SerializeField] public SO_Dialogue[] NPC_Dialogue_Roots;
    public bool isFrozen = false; // Needed for animations

    // Other scripts
    [SerializeField] private PlayerInventory playerInv;
    [SerializeField] private PlayerData playerData;
    [SerializeField] private UIInventory uiInventory;

    // FMS
    public NPCState currentState = NPCState.Idle;

    public void Start()
    {
        playerData = FindObjectOfType<PlayerData>();
        playerInv = FindObjectOfType<PlayerInventory>();
        uiInventory = FindObjectOfType<UIInventory>();
        promptE = FindObjectOfType<promptE>();
        
        originalSpeed = moveSpeed;
        ChooseNextWaypoint();

        currentState = NPCState.Moving;
    }

    void Update()
    {
        if (isFrozen)
        {
            currentState = NPCState.Talking;
            return;
        }

        switch (currentState)
        {
            case NPCState.Idle:
                break;

            case NPCState.Moving:
                HandleMovement();
                break;

            case NPCState.Waiting:
                break;

            case NPCState.Talking:
                break;
        }

        Debug.Log(npcName + " State = " + currentState);
    }

    void HandleMovement()
    {
        if (path == null || currentPathIndex >= path.Count) return;

        Waypoint target = path[currentPathIndex];
        Vector3 targetPosition = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z);

        float distanceToTarget = Vector3.Distance(transform.position, targetPosition);
        if (distanceToTarget < 1f)
        {
            moveSpeed = Mathf.Lerp(moveSpeed, 0f, Time.deltaTime * 3f);
        }

        MoveNPC(targetPosition);

        if (distanceToTarget < 0.1f)
        {
            currentPathIndex++;
            if (currentPathIndex >= path.Count)
            {
                float rand = Random.Range(0f, 1f);
                if (rand < 0.6f)
                {
                    StartCoroutine(WaitAtWaypoint());
                }
                else
                {
                    StartCoroutine(ExtraActionAtWaypoint());
                }
            }
        }
    }

    void MoveNPC(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * originalSpeed);
        }

        rb.MovePosition(transform.position + direction * Time.deltaTime * moveSpeed);
    }

    void ChooseNextWaypoint()
    {
        if (currentWaypoint == null)
        {
            // Debug.LogError($"{gameObject.name} has no starting waypoint set!");
            return;
        }

        var options = currentWaypoint.neighbors;
        if (options == null || options.Count == 0)
        {
            // Debug.LogWarning($"{currentWaypoint.name} has no neighbors.");
            return;
        }

        Waypoint next = options[Random.Range(0, options.Count)];
        path = AStarPathfinder.FindPath(currentWaypoint, next);

        if (path == null)
        {
            // Debug.LogError($"No path found from {currentWaypoint.name} to {next.name}");
            return;
        }

        currentPathIndex = 0;
        currentWaypoint = next;

        // Debug.Log($"{gameObject.name} pathing from {currentWaypoint.name} to {next.name}");
    }

    private IEnumerator WaitAtWaypoint()
    {
        currentState = NPCState.Waiting;
        float waitTime = Random.Range(2f, 5f);
        yield return new WaitForSeconds(waitTime);

        moveSpeed = originalSpeed;
        ChooseNextWaypoint();
        currentState = NPCState.Moving;
    }

    public void StopMovement()
    {
        isFrozen = true;
        rb.velocity = Vector3.zero;
        currentState = NPCState.Talking;
    }

    public void ResumeMovement()
    {
        isFrozen = false;
        currentState = NPCState.Moving;
    }

    public void FaceTarget(Vector3 targetPosition)
    {
        StopCoroutine(nameof(SmoothFaceTarget));
        StartCoroutine(SmoothFaceTarget(targetPosition));
    }

    private IEnumerator SmoothFaceTarget(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        direction.y = 0;

        if (direction == Vector3.zero) yield break;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        float rotationSpeed = 5f;

        while (Quaternion.Angle(transform.rotation, targetRotation) > 0.5f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            yield return null;
        }

        transform.rotation = targetRotation;
    }

    private IEnumerator ExtraActionAtWaypoint()
    {
        currentState = NPCState.ExtraAction;
        yield return new WaitForSeconds(6f);

        moveSpeed = originalSpeed;
        ChooseNextWaypoint();
        currentState = NPCState.Moving;
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }




    // DIALOGUE CODE

    public int DetermineRootDialogue()
    {
        if (npcID == 1) // Engineer
        {
            if (!PlayerData.hasBegunQuest && !PlayerData.hasFinishedQuest && !hasInteractedWithPlayer)
            {
                hasInteractedWithPlayer = true;
                return 0; // Interacting with player for the first time.
            }
            else if (!PlayerData.hasBegunQuest && !PlayerData.hasFinishedQuest && hasInteractedWithPlayer)
            {
                return 1; // Has interacted with player before, still doesn't know about the boat tho.
            }
            else if (PlayerData.hasBegunQuest && !PlayerData.hasFinishedQuest && hasInteractedWithPlayer)
            {
                return 2; // Quest already began, he's waiting for the items now.
            }
            else if (PlayerData.hasBegunQuest && PlayerData.hasFinishedQuest && hasInteractedWithPlayer)
            {
                return 3; // Quest has ended, now he welcomes you back to talk.
            }
        }

        return 0;
    }

    public int DetermineNextDialogueThroughOption(DialogueOption option)
    {
        if (npcID == 1) // Engineer
        {
            if (option.optionID == 0) // "I need help with the boat."
            {
                PlayerData.hasBegunQuest = true;
                return 0; // Engineer tells you what items you need to find, where to find them and then the quest begins.
            }

            if (option.optionID == 1) // "I've got the items."
            {
                if (PlayerData.amountQuestItemsCollected == 0)
                {
                    return 0; // Engineer laughs, thinking you are joking, since you were barely gone and clearly have no items.
                }
                else if (PlayerData.amountQuestItemsCollected < 3 && PlayerData.amountQuestItemsCollected != 0)
                {
                    return 1; // Engineer tells you that you still have items left to find and to go find them before coming back.
                }
                else if (PlayerData.amountQuestItemsCollected == 3)
                {
                    PlayerData.hasFinishedQuest = true;

                    playerData.RemoveItem(1);
                    playerData.RemoveItem(2);
                    playerData.RemoveItem(3);

                    playerData.CollectPart(4);

                    uiInventory.SetItemInvSlotImage(0);
                    uiInventory.SetItemInvSlotImage(1);
                    uiInventory.SetItemInvSlotImage(2);
                    uiInventory.SetItemInvSlotImage(3);

                    return 2; // Engineer accepts your items and gives you the boat part in return.
                }
            }
        }

        else if (npcID == 0) // Nurse
        {
            if (option.optionID == 1) // "I need more medicine."
            {
                if (playerInv.healthPotionAmount < 3 && playerData.playerHP < PlayerData.playerMaxHP)
                {
                    playerInv.healthPotionAmount = 3;
                    playerData.playerHP = PlayerData.playerMaxHP;
                    return 0; // You get medicine and get cured.
                }
                else if (playerInv.healthPotionAmount >= 3 && playerData.playerHP < PlayerData.playerMaxHP)
                {
                    playerData.playerHP = PlayerData.playerMaxHP;
                    return 1; // You get cured and a get a warning.
                }
                else if (playerInv.healthPotionAmount < 3 && playerData.playerHP == PlayerData.playerMaxHP)
                {
                    playerInv.healthPotionAmount = 3;
                    return 2; // You get potions and get a warning.
                }
                else if (playerInv.healthPotionAmount >= 3 && playerData.playerHP == PlayerData.playerMaxHP)
                {
                    return 3; // You get called a greedy prick and get told to get a life.
                }
            }
        }

        return 0;
    }
}