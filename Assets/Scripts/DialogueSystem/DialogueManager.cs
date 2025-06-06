using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    // EVERYTHING AUDIO HAS TO BE ADDED MANUALLY IN INSPECTOR FOR EVERY INSTANCE OF THIS FILE
    public AudioSource audioSourceMenus;
    public AudioClip NpcTalkSound;

    [SerializeField] private GameObject Canvas;
    [SerializeField] private TMP_Text NameText;
    [SerializeField] private TMP_Text TextObject;
    [SerializeField] private Transform ButtonContainer;
    [SerializeField] private Button ButtonPrefab;
    public promptE promptE;

    [SerializeField] private PlayerData player;
    [SerializeField] private NPC_Behaviour npc;

    [SerializeField] private PauseMenu pauseMenu;

    [SerializeField] public float DialogueTextSpeed;
    
    private bool isPromptEActive = false;
    private bool inDialogue = false;

    private SO_Dialogue CurrentDialogue;
    private Action dialogueComplete;
    private bool isInputReceived;
    private List<Button> activeButtons = new List<Button>();
    private NPC_Behaviour activeNPC;

    [SerializeField] public CinemachineFreeLook cameraControl;

    private Transform playerTransform;

    private void Start()
    {
        promptE = FindObjectOfType<promptE>();
        player = FindObjectOfType<PlayerData>();

        pauseMenu = FindObjectOfType<PauseMenu>();

        if (npc.NPC_Dialogue_Roots == null)
        {
            Debug.LogError("No DebugDialog assigned!");
        }

        if (player == null)
        {
            Debug.LogError("No player in DebugDialog assigned!");
        }

        if (cameraControl == null)
        {
            cameraControl = FindObjectOfType<CinemachineFreeLook>();
        }

        playerTransform = player.transform;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && Canvas.activeSelf == false && !pauseMenu.isPaused)
        {
            NPC_Behaviour closestNPC = FindClosestNPC();

            if (closestNPC != null && closestNPC.NPC_Dialogue_Roots != null)
            {
                activeNPC = closestNPC;
                int rootIndex = closestNPC.DetermineRootDialogue();
                if (rootIndex >= 0 && rootIndex < closestNPC.NPC_Dialogue_Roots.Length)
                {
                    promptE.PromptE_Disable();
                    isPromptEActive = false;
                    inDialogue = true;
                    cameraControl.gameObject.SetActive(false);
                    StartDialogue(closestNPC.NPC_Dialogue_Roots[rootIndex], null);
                }
                else
                {
                    Debug.LogWarning("Invalid root dialogue index for " + closestNPC.npcName);
                }
            }
        }

        NPC_ControlPromptE();
    }
    private NPC_Behaviour FindClosestNPC()
    {
        NPC_Behaviour[] allNPCs = FindObjectsOfType<NPC_Behaviour>();
        NPC_Behaviour closestNPC = null;
        float closestDistance = Mathf.Infinity;

        foreach (NPC_Behaviour npc in allNPCs)
        {
            float dist = Vector3.Distance(player.transform.position, npc.transform.position);
            if (dist <= npc.interactionRadius && dist < closestDistance)
            {
                closestDistance = dist;
                closestNPC = npc;
            }
        }

        return closestNPC;
    }

    private void NPC_ControlPromptE()
    {
        NPC_Behaviour closestNPC = FindClosestNPC();

        if (closestNPC == null)
        {
            if (isPromptEActive)
            {
                promptE.PromptE_Disable();
                isPromptEActive = false;
            }
            return;
        }

        float distance = Vector3.Distance(player.transform.position, closestNPC.transform.position);
        bool withinRange = distance <= closestNPC.interactionRadius;

        if (withinRange && !isPromptEActive && !inDialogue)
        {
            promptE.PromptE_Show();
            isPromptEActive = true;
        }
        else if (!withinRange && isPromptEActive)
        {
            promptE.PromptE_Disable();
            isPromptEActive = false;
        }
    }



    private Coroutine dialogueCoroutine;
    public void StartDialogue(SO_Dialogue DialogueToPlay, Action onDialogueComplete)
    {
        if (dialogueCoroutine != null)
        {
            StopCoroutine(dialogueCoroutine);
        }

        if (player.TryGetComponent<ThirdPersonMovement>(out var movementScript))
        {
            movementScript.FreezePlayer(activeNPC.transform);
        }

        CurrentDialogue = DialogueToPlay;
        dialogueComplete = onDialogueComplete;

        if (activeNPC != null && NameText != null)
            NameText.text = activeNPC.npcName;

        if (activeNPC != null)
        {
            activeNPC.StopMovement();
            activeNPC.FaceTarget(playerTransform.position);
        }

        dialogueCoroutine = StartCoroutine(PlayDialogue());
    }

    private IEnumerator PlayDialogue()
    {
        Canvas.SetActive(true);
        ClearButtons();

        foreach (string dialogueLine in CurrentDialogue.Dialogues)
        {
            audioSourceMenus.pitch = UnityEngine.Random.Range(0.8f, 1.2f);
            audioSourceMenus.clip = NpcTalkSound;
            audioSourceMenus.Play();
            
            TextObject.text = "";

            foreach (char letter in dialogueLine)
            {
                
                TextObject.text += letter;
                yield return new WaitForSeconds(DialogueTextSpeed);
            }

            isInputReceived = false;
            while (!isInputReceived)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    isInputReceived = true;
                }
                yield return null;
            }
        }

        if (CurrentDialogue.DialogueOptions.Length > 0)
        {
            ShowDialogueOptions();
        }
        else
        {
            EndDialogue();
        }
    }

    private void ShowDialogueOptions()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        
        ClearButtons();

        for (int i = 0; i < CurrentDialogue.DialogueOptions.Length; i++)
        {
            DialogueOption option = CurrentDialogue.DialogueOptions[i];
            Button newButton = Instantiate(ButtonPrefab, ButtonContainer);
            newButton.transform.GetChild(0).GetComponent<TMP_Text>().text = option.DialogueOptionText;

            DialogueOption capturedOption = option;
            newButton.onClick.AddListener(() => OnOptionSelected(capturedOption));

            activeButtons.Add(newButton);
        }
    }

    private void OnOptionSelected(DialogueOption option)
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        
        ClearButtons();

        if (option.NextDialoguePossibilities != null && option.NextDialoguePossibilities.Length == 1)
        {
            StartDialogue(option.NextDialoguePossibilities[0], dialogueComplete);
            return;
        }

        int selectedIndex = activeNPC.DetermineNextDialogueThroughOption(option);

        if (selectedIndex >= 0 && selectedIndex < option.NextDialoguePossibilities.Length)
        {
            StartDialogue(option.NextDialoguePossibilities[selectedIndex], dialogueComplete);
        }
        else
        {
            EndDialogue();
        }
    }

    private void ClearButtons()
    {
        foreach (Button btn in activeButtons)
        {
            Destroy(btn.gameObject);
        }
        activeButtons.Clear();
    }

    private void EndDialogue()
    {
        inDialogue = false;
        Canvas.SetActive(false);
        activeNPC?.ResumeMovement();
        dialogueComplete?.Invoke();

        cameraControl.gameObject.SetActive(true);

        if (player.TryGetComponent<ThirdPersonMovement>(out var movementScript))
        {
            movementScript.UnfreezePlayer();
        }
    }
}