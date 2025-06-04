using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    // EVERYTHING AUDIO HAS TO BE ADDED MANUALLY IN INSPECTOR FOR EVERY INSTANCE OF THIS FILE
    public AudioSource audioSourceMenus;
    public AudioClip clickSound;
    
    public GameObject pauseMenuUI;
    public GameObject gameUI;
    public GameObject settingsMenu;
    public Slider volumeSlider;
    public Slider graphicsSlider;
    public ThirdPersonMovement playerMovement;

    [SerializeField] private float volumeSliderCurrentValue = 1f;
    [SerializeField] private float volumeSliderNewValue;
    [SerializeField] private float graphicsSliderCurrentValue = 1f;
    [SerializeField] private float graphicsSliderNewValue;

    public bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !playerMovement.isFrozen)
        {
            volumeSlider.value = volumeSliderCurrentValue;
            graphicsSlider.value = graphicsSliderCurrentValue;
            settingsMenu.SetActive(false);
            
            if (isPaused) Resume();
            else Pause();
        }
        
        volumeSliderNewValue = volumeSlider.value;
        graphicsSliderNewValue = graphicsSlider.value;
    }

    public void Resume()
    {
        audioSourceMenus.clip = clickSound;
        audioSourceMenus.Play();
        
        pauseMenuUI.SetActive(false);
        gameUI.SetActive(true);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        Time.timeScale = 1f; // Resume game time
        isPaused = false;
    }

    void Pause()
    {
        audioSourceMenus.clip = clickSound;
        audioSourceMenus.Play();
        
        pauseMenuUI.SetActive(true);
        gameUI.SetActive(false);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        Time.timeScale = 0f; // Freeze game time
        isPaused = true;
    }

    public void EnterSettings()
    {
        audioSourceMenus.clip = clickSound;
        audioSourceMenus.Play();
        
        settingsMenu.SetActive(true);
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }
    
    public void Settings_SaveButton()
    {
        audioSourceMenus.clip = clickSound;
        audioSourceMenus.Play();
        
        volumeSliderCurrentValue = volumeSliderNewValue;
        volumeSlider.value = volumeSliderCurrentValue;
        graphicsSliderCurrentValue = graphicsSliderNewValue;
        graphicsSlider.value = graphicsSliderCurrentValue;
        
        settingsMenu.SetActive(false);
    }

    public void Settings_CancelButton()
    {
        audioSourceMenus.clip = clickSound;
        audioSourceMenus.Play();
        
        volumeSlider.value = volumeSliderCurrentValue;
        graphicsSlider.value = graphicsSliderCurrentValue;
        
        settingsMenu.SetActive(false);
    }
}
