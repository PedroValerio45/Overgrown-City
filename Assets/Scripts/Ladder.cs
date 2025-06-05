using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class Ladder : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public Renderer renderer;
    public Material materialNormal;
    public Material materialExtra;
    public GameObject poster;

    private bool isPlaying;
    private bool playerInsideTrigger;

    void Start()
    {
        videoPlayer.loopPointReached += OnVideoFinished;
    }

    void Update()
    {
        if (playerInsideTrigger && Input.GetKeyDown(KeyCode.M) && !isPlaying)
        {
            renderer.material = materialExtra;
            videoPlayer.Play();
            isPlaying = true;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInsideTrigger = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInsideTrigger = false;

            if (isPlaying)
            {
                videoPlayer.Stop();
                renderer.material = materialNormal;
                isPlaying = false;
            }
        }
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        renderer.material = materialNormal;
        isPlaying = false;
    }
}