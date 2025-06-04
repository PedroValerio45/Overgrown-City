using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // Add this line

public class GameOverTrans : MonoBehaviour
{
    public PlayerData playerData;
    public Image gameOverImage;
    public float fadeDuration;

    private bool hasFadedIn = false;

    void Start()
    {
        if (gameOverImage != null)
        {
            Color c = gameOverImage.color;
            c.a = 0f;
            gameOverImage.color = c;
            gameOverImage.gameObject.SetActive(false); // Hide initially
        }

        if (playerData == null)
        {
            playerData = FindObjectOfType<PlayerData>();
        }
    }

    void Update()
    {
        if (!hasFadedIn && playerData != null && playerData.playerHP <= 0)
        {
            hasFadedIn = true;
            gameOverImage.gameObject.SetActive(true);
            StartCoroutine(FadeIn());
        }
    }

    IEnumerator FadeIn()
    {
        float elapsed = 0f;
        Color c = gameOverImage.color;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            c.a = Mathf.Clamp01(elapsed / fadeDuration);
            gameOverImage.color = c;
            yield return null;
        }

        c.a = 1f;
        gameOverImage.color = c;

        yield return new WaitForSeconds(0.4f);

        SceneManager.LoadScene("GameOver");
    }
}
