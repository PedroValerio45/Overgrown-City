using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ManuMenu : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip soundBell;
    [SerializeField] private AudioClip soundError;
    
    [SerializeField] private Button continueButton;
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button exitGameButton;
    
    [SerializeField] private float frameInterval = 0.5f; // time between flashes
    [SerializeField] private float flashDuration = 1f; // flash duration
    private int frameCount = 0; // Tracks frames
    private int buttonPressedNumber = 0;
    private bool playCanFlash = false;


    void Start()
    {
    }
    void Update()
    {
        if (buttonPressedNumber != 0)
        {
            frameCount++;

            // Toggle visibility every `frameInterval` frames
            if (frameCount >= frameInterval && playCanFlash)
            {
                ToggleButtonVisibility();
                frameCount = 0; // Reset the frame counter
            }
        }
    }
    
    public void PlayGame()
    {
        buttonPressedNumber = 1;
        ReadPlayerFile();
        if (ReadPlayerFile() != -1)
        {
            playCanFlash = true;
            
            StartCoroutine(FlashForDuration(flashDuration));
        }
        else
        {
            audioSource.clip = soundError;
            audioSource.Play();
            
            print("File does not exist.");
        }
        
    }

    public void OptionsMenu()
    {
        buttonPressedNumber = 2;
        StartCoroutine(FlashForDuration(flashDuration));
    }

    public void QuitGame()
    {
        buttonPressedNumber = 3;
        StartCoroutine(FlashForDuration(flashDuration));
    }

    void ToggleButtonVisibility()
    {
        switch (buttonPressedNumber)
        {
            case 1:
                if (continueButton != null) continueButton.GetComponent<Image>().enabled = !continueButton.GetComponent<Image>().enabled;
                break;
            
            case 2:
                if (newGameButton != null) newGameButton.GetComponent<Image>().enabled = !newGameButton.GetComponent<Image>().enabled;
                break;
            
            case 3:
                if (settingsButton != null) settingsButton.GetComponent<Image>().enabled = !settingsButton.GetComponent<Image>().enabled;
                break;
            
            case 4:
                if (exitGameButton != null) exitGameButton.GetComponent<Image>().enabled = !exitGameButton.GetComponent<Image>().enabled;
                break;
        }
    }

    IEnumerator FlashForDuration(float duration)
    {
        audioSource.clip = soundBell;
        audioSource.Play();
        
        yield return new WaitForSeconds(duration); 

        if (buttonPressedNumber == 1)
        {
            SceneManager.LoadScene(1, LoadSceneMode.Single);
        }
        else if (buttonPressedNumber == 2)
        {
            print("No one can choose who their are in this world. Your name is... AMONGUS");
        }
        else if (buttonPressedNumber == 3)
        {
            Application.Quit();
        }

        buttonPressedNumber = 0;
    }
    
    private int ReadPlayerFile()
    {
        string fileName = "playerID.txt";
        string filePath = Path.Combine(Application.dataPath, fileName);
        int playerID = -1; // Default value in case of failure

        // Read the content from the file
        if (File.Exists(filePath))
        {
            string fileContent = File.ReadAllText(filePath);

            // Convert the content back to an integer
            if (int.TryParse(fileContent, out int readID))
            {
                Debug.Log($"Read number {readID} from file.");
                playerID = readID;
            }
            else
            {
                Debug.LogError("Failed to parse the content as an integer.");
            }
        }
        
        return playerID;
    }
}