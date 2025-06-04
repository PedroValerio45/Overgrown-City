using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.IO;

public class GameOver : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public CanvasGroup uiGroup;
    public float fadeDuration = 1f;

    public AudioSource audioSource;
    public AudioClip clickSound;

    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        
        uiGroup.alpha = 0f;
        uiGroup.gameObject.SetActive(false);

        videoPlayer.loopPointReached += OnVideoFinished;
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        StartCoroutine(FadeInUI());
    }

    IEnumerator FadeInUI()
    {
        uiGroup.gameObject.SetActive(true);
        uiGroup.interactable = true;
        uiGroup.blocksRaycasts = true;

        float time = 0f;
        while (time < fadeDuration)
        {
            uiGroup.alpha = Mathf.Lerp(0f, 1f, time / fadeDuration);
            time += Time.deltaTime;
            yield return null;
        }

        uiGroup.alpha = 1f;
    }


    public void Continue()
    {
        audioSource.clip = clickSound;
        audioSource.Play();

        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }

    public void Quit()
    {
        audioSource.clip = clickSound;
        audioSource.Play();

        SceneManager.LoadScene("MainMenu");
    }

    public void NewGame()
    {
        audioSource.clip = clickSound;
        audioSource.Play();

        DeletePlayerFile_Collectables();
        SceneManager.LoadScene(1, LoadSceneMode.Single);
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
