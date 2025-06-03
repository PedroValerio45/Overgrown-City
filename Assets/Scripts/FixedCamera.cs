using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedCamera : MonoBehaviour
{
    public GameObject marketCamera;
    public GameObject freeCamera;
    public Transform mainCamera;

    [SerializeField] private bool isMarket;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            marketCamera.SetActive(true);
            freeCamera.SetActive(false);

            if (isMarket)
            {
                
                mainCamera.rotation = Quaternion.Euler(27, 0, 0);
            }
            else
            {
                mainCamera.rotation = Quaternion.Euler(0, 220, 0);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        marketCamera.SetActive(false);
        freeCamera.SetActive(true);
    }
}
