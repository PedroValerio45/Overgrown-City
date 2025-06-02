using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public GameObject gameUI;
    public ThirdPersonMovement playerMovement;

    public bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !playerMovement.isFrozen)
        {
            if (isPaused) Resume();
            else Pause();
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        gameUI.SetActive(true);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        Time.timeScale = 1f; // Resume game time
        isPaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        gameUI.SetActive(false);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        Time.timeScale = 0f; // Freeze game time
        isPaused = true;
    }

    public void EnterSettings()
    {
        Time.timeScale = 1f;
        // insert settings code here
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;
        Application.Quit();
    }
}
