using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ManuMenu : MonoBehaviour
{
    public AudioSource audioSource;
    // [SerializeField] private AudioClip soundBell;
    // [SerializeField] private AudioClip soundError;
    public GameObject settingsMenu;
    public Slider volumeSlider;
    public Slider graphicsSlider;
    
    [SerializeField] private Button continueButton;
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button exitGameButton;
    [SerializeField] private Button saveButton;
    [SerializeField] private Button cancelButton;

    [SerializeField] private float volumeSliderCurrentValue = 1f;
    [SerializeField] private float volumeSliderNewValue;
    [SerializeField] private float graphicsSliderCurrentValue = 1f;
    [SerializeField] private float graphicsSliderNewValue;
    
    private int frameCount; // Tracks frames
    private int buttonPressedNumber;
    private bool playCanFlash;

    void Update()
    {
        /* if (buttonPressedNumber != 0)
        {
            frameCount++;

            // Toggle visibility every `frameInterval` frames
            if (frameCount >= frameInterval && playCanFlash)
            {
                ToggleButtonVisibility();
                frameCount = 0; // Reset the frame counter
            }
        } */
        
        volumeSliderNewValue =  volumeSlider.value;
        graphicsSliderNewValue = graphicsSlider.value;
    }
    
    public void ContinueGame()
    {
        buttonPressedNumber = 1;
        if (ReadPlayerFile_Collectables())
        {
            SceneManager.LoadScene(1, LoadSceneMode.Single);
        }
        else
        {
            // audioSource.clip = soundError;
            // audioSource.Play();
        }
    }
    
    public void NewGame()
    {
        buttonPressedNumber = 2;
        DeletePlayerFile_Collectables();
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }

    public void OptionsMenu()
    {
        settingsMenu.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Setings_SaveButton()
    {
        volumeSliderCurrentValue = volumeSliderNewValue;
        volumeSlider.value = volumeSliderCurrentValue;
        graphicsSliderCurrentValue = graphicsSliderNewValue;
        graphicsSlider.value = graphicsSliderCurrentValue;
        settingsMenu.SetActive(false);
    }

    public void Setings_CancelButton()
    {
        volumeSlider.value = volumeSliderCurrentValue;
        graphicsSlider.value = graphicsSliderCurrentValue;
        settingsMenu.SetActive(false);
    }

    void ToggleButtonVisibility()
    {
        switch (buttonPressedNumber)
        {
            case 1:
                continueButton.GetComponent<Image>().enabled = !continueButton.GetComponent<Image>().enabled;
                break;
            
            case 2:
                newGameButton.GetComponent<Image>().enabled = !newGameButton.GetComponent<Image>().enabled;
                break;
            
            case 3:
                settingsButton.GetComponent<Image>().enabled = !settingsButton.GetComponent<Image>().enabled;
                break;
            
            case 4:
                exitGameButton.GetComponent<Image>().enabled = !exitGameButton.GetComponent<Image>().enabled;
                break;
            
            case 5:
                saveButton.GetComponent<Image>().enabled = !saveButton.GetComponent<Image>().enabled;
                break;
            
            case 6:
                cancelButton.GetComponent<Image>().enabled = !cancelButton.GetComponent<Image>().enabled;
                break;
        }
    }

    // (UNUSED)
    IEnumerator FlashForDuration(float duration)
    {
        // audioSource.clip = soundBell;
        // audioSource.Play();
        
        yield return new WaitForSeconds(duration); 

        if (buttonPressedNumber == 1 || buttonPressedNumber == 2)
        {
            SceneManager.LoadScene(1, LoadSceneMode.Single);
        }
        else if (buttonPressedNumber == 3)
        {
            settingsMenu.SetActive(true);
        }
        else if (buttonPressedNumber == 4)
        {
            Application.Quit();
        }
        else if (buttonPressedNumber == 5 ||  buttonPressedNumber == 6)
        {
            settingsMenu.SetActive(false);
        }

        buttonPressedNumber = 0;
    }
    
    public bool ReadPlayerFile_Collectables()
    {
        string filePath = Path.Combine(Application.dataPath, "playerCollectables.txt");

        if (File.Exists(filePath))
        {
            
            print("Menu: File found.");
            return true;
        }
        else
        {
            print("Menu: File does not exist.");
            return false;
        }
    }
    
    public void DeletePlayerFile_Collectables()
    {
        string filePath = Path.Combine(Application.dataPath, "playerCollectables.txt");

        if (File.Exists(filePath))
        {
            try
            {
                File.Delete(filePath);
                Debug.Log("Collectables file deleted successfully.");
            }
            catch (System.Exception e)
            {
                Debug.LogError("Failed to delete file: " + e.Message);
            }
        }
        else
        {
            Debug.Log("File does not exist, nothing to delete.");
        }
    }
}