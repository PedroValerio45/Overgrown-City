using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Behaviour : MonoBehaviour
{
    [SerializeField] public CapsuleCollider cc;
    [SerializeField] private Rigidbody rb;
    public promptE promptE;

    // A* pathfinding stuff
    [SerializeField] public Waypoint currentWaypoint;
    [SerializeField] public float moveSpeed;
    private bool isWaiting = false;
    private float originalSpeed;
    private List<Waypoint> path;
    private int currentPathIndex = 0;

    // Dialogue stuff
    [SerializeField] public float interactionRadius;
    [SerializeField] public int npcID;
    [SerializeField] public string npcName;
    [SerializeField] public static bool hasInteractedWithPlayer;
    [SerializeField] public SO_Dialogue[] NPC_Dialogue_Roots;
    private bool isFrozen = false;

    // Other scripts
    [SerializeField] private PlayerInventory playerInv;
    [SerializeField] private PlayerData playerData;


    public void Start()
    {
        playerData = FindObjectOfType<PlayerData>();
        promptE = FindObjectOfType<promptE>();
        
        originalSpeed = moveSpeed;
        ChooseNextWaypoint();
    }

    void Update()
    {
        if (isWaiting || isFrozen) return;

        if (path != null && currentPathIndex < path.Count)
        {
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
                    StartCoroutine(WaitAtWaypoint());
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
            Debug.LogError($"{gameObject.name} has no starting waypoint set!");
            return;
        }

        var options = currentWaypoint.neighbors;

        if (options == null || options.Count == 0)
        {
            Debug.LogWarning($"{currentWaypoint.name} has no neighbors.");
            return;
        }

        Waypoint next = options[Random.Range(0, options.Count)];
        path = AStarPathfinder.FindPath(currentWaypoint, next);

        if (path == null)
        {
            Debug.LogError($"No path found from {currentWaypoint.name} to {next.name}");
            return;
        }

        currentPathIndex = 0;
        currentWaypoint = next;

        Debug.Log($"{gameObject.name} pathing from {currentWaypoint.name} to {next.name}");
    }

    private IEnumerator WaitAtWaypoint()
    {
        isWaiting = true;
        float waitTime = Random.Range(2f, 5f);
        Debug.Log($"{gameObject.name} is waiting for {waitTime} seconds.");

        yield return new WaitForSeconds(waitTime);

        isWaiting = false;
        moveSpeed = originalSpeed;
        ChooseNextWaypoint();
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
    public void StopMovement()
    {
        isFrozen = true;
        rb.velocity = Vector3.zero;
    }

    public void ResumeMovement()
    {
        isFrozen = false;
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }
}
