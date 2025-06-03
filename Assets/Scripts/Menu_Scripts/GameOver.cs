using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
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

        // Ensure full opacity at the end
        c.a = 1f;
        gameOverImage.color = c;
    }
}