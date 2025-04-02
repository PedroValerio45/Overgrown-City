using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    /*
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip text_sfx;
    
    [SerializeField] private GameObject Canvas;
    [SerializeField] private TMP_Text TextObject;

    [SerializeField] private Button Button1;
    [SerializeField] private Button Button2;
    [SerializeField] private Button Button3;

    [SerializeField] public float DialogueTextSpeed;
    
    public SO_Dialogue DebugDialog;
    private SO_Dialogue CurrentDialogue;

    private Action dialogueComplete;
    private bool isInputReceived;

    public void StartDialogue(SO_Dialogue DialogueToPlay, Action onDialogueComplete)
    {
        CurrentDialogue = DialogueToPlay;
        dialogueComplete = onDialogueComplete;
        StartCoroutine(PlayDialogue());
    }

    private IEnumerator PlayDialogue()
    {
        Canvas.SetActive(true);

        Button1.gameObject.SetActive(false);
        Button2.gameObject.SetActive(false);
        Button3.gameObject.SetActive(false);

        for (int DialogueNumber = 0; DialogueNumber < CurrentDialogue.Dialogues.Length; ++DialogueNumber)
        {
            TextObject.text = "";
            string CurrentDialogueString = CurrentDialogue.Dialogues[DialogueNumber];

            for (int DialogueLetterIndex = 0; DialogueLetterIndex < CurrentDialogueString.Length; ++DialogueLetterIndex)
            {
                audioSource.clip = text_sfx;
                audioSource.Play();
                
                TextObject.text += CurrentDialogueString[DialogueLetterIndex];
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

        if (CurrentDialogue.DialogueOptions.Length >= 2 && Array.TrueForAll(CurrentDialogue.DialogueOptions, option => option != null))
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
        Button1.gameObject.SetActive(true);
        Button2.gameObject.SetActive(true);
        Button3.gameObject.SetActive(true);

        Button1.transform.GetChild(0).GetComponent<TMP_Text>().text = CurrentDialogue.DialogueOptions[0].DialogueOptionText;
        Button2.transform.GetChild(0).GetComponent<TMP_Text>().text = CurrentDialogue.DialogueOptions[1].DialogueOptionText;
        Button3.transform.GetChild(0).GetComponent<TMP_Text>().text = CurrentDialogue.DialogueOptions[2].DialogueOptionText;

        Button1.onClick.AddListener(OnButton1Clicked);
        Button2.onClick.AddListener(OnButton2Clicked);
        Button3.onClick.AddListener(OnButton3Clicked);
    }

    private void EndDialogue()
    {
        Canvas.SetActive(false);

        if (dialogueComplete != null)
        {
            dialogueComplete.Invoke();
        }
    }

    private void OnButton1Clicked()
    {
        Button1.onClick.RemoveListener(OnButton1Clicked);
        StartDialogueWithBranch(CurrentDialogue.DialogueOptions[0]);
    }

    private void OnButton2Clicked()
    {
        Button2.onClick.RemoveListener(OnButton2Clicked);
        StartDialogueWithBranch(CurrentDialogue.DialogueOptions[1]);
    }

    private void OnButton3Clicked()
    {
        Button3.onClick.RemoveListener(OnButton3Clicked);
        StartDialogueWithBranch(CurrentDialogue.DialogueOptions[2]);
    }

    private void StartDialogueWithBranch(DialogueOption option)
    {
        HideAndClearButtons();

        SO_Dialogue nextDialogue = option.NextDialogue;

        if (nextDialogue != null && nextDialogue.NextDialoguePossibilities.Length >= 2)
        {
            StartCoroutine(PlayRootDialogueAndBranch(nextDialogue));
        }
        else
        {
            StartDialogue(nextDialogue, dialogueComplete);
        }
    }

    private void HideAndClearButtons()
    {
        Button1.gameObject.SetActive(false);
        Button1.onClick.RemoveAllListeners();

        Button2.gameObject.SetActive(false);
        Button2.onClick.RemoveAllListeners();

        Button3.gameObject.SetActive(false);
        Button3.onClick.RemoveAllListeners();
    }

    private IEnumerator PlayRootDialogueAndBranch(SO_Dialogue nextDialogue)
    {
        yield return StartCoroutine(PlayDialogueFromRoot(nextDialogue));
        HandleBranching(nextDialogue);
    }

    private IEnumerator PlayDialogueFromRoot(SO_Dialogue nextDialogue)
    {
        for (int i = 0; i < nextDialogue.Dialogues.Length; ++i)
        {
            TextObject.text = "";
            string dialogueString = nextDialogue.Dialogues[i];

            for (int j = 0; j < dialogueString.Length; ++j)
            {
                TextObject.text += dialogueString[j];
                yield return new WaitForSeconds(0.05f);
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
    }

    private void HandleBranching(SO_Dialogue nextDialogue)
    {
        if (PlayerController.Instance.memoryCounter > 0)
        {
            StartDialogue(nextDialogue.NextDialoguePossibilities[0], dialogueComplete);
            PlayerController.Instance.memoryCounter = 0;
        }
        else
        {
            StartDialogue(nextDialogue.NextDialoguePossibilities[1], dialogueComplete);
        } 
    }*/
}