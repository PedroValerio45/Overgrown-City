using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor.Experimental;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    // STUFF IN THIS SCENE IS TO BE SAVES BETWEEN SCENES

    // this has to be added in all unity scenes that are relevant!
    public UI uiScript;

    // Amount of boat parts collected (will prob go unused)
    // public static int partsCollectedAmount = 0;
    
    // List of boat parts collected
    public static List<int> partsCollected = new List<int>();
    
    // PLAYER STATS
    public static int playerMaxHP = 4;
    public static int playerHP = 4;
    public static string playerAura = "3000 gazillions";

    void Start()
    {
        uiScript.SetMaxHealth(playerMaxHP);
        uiScript.SetHealth(playerHP);
        Debug.Log(playerMaxHP);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            playerHP += 1;
            uiScript.SetHealth(playerHP);
            Debug.Log(playerHP);
        }
        else if (Input.GetKeyDown(KeyCode.O))
        {
            playerHP -= 1;
            uiScript.SetHealth(playerHP);
            Debug.Log(playerHP);
        }
    }

    // PLAYER COLLECTABLES
    public void CreateOrWritePlayerFile_Collectables()
    {
        string filePath = Path.Combine(Application.dataPath, "playerCollectables.txt");

        try
        {
            File.WriteAllLines(filePath, partsCollected.ConvertAll(d => d.ToString()));
            Debug.Log("File saved successfully at: " + filePath);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to save file: " + e.Message);
        }
    }
    
    public List<int> ReadPlayerFile_Collectables()
    {
        // string fileName = "PlayerData.txt";
        string filePath = Path.Combine(Application.dataPath, "playerCollectables.txt");

        if (File.Exists(filePath))
        {
            string[] lines = File.ReadAllLines(filePath);
            
            partsCollected.Clear();

            foreach (string line in lines)
            {
                if (int.TryParse(line, out int number))
                {
                    partsCollected.Add(number);
                }
            }
        }
        else
        {
            print("File does not exist.");
        }

        return partsCollected;
    }
    
    public void CreateOrWritePlayerFile_Health()
    {
        string filePath = Path.Combine(Application.dataPath, "playerCollectables.txt");

        try
        {
            File.WriteAllLines(filePath, partsCollected.ConvertAll(d => d.ToString()));
            Debug.Log("File saved successfully at: " + filePath);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to save file: " + e.Message);
        }
    }
    
    public List<int> ReadPlayerFile_Health()
    {
        // string fileName = "PlayerData.txt";
        string filePath = Path.Combine(Application.dataPath, "playerCollectables.txt");

        if (File.Exists(filePath))
        {
            string[] lines = File.ReadAllLines(filePath);
            
            partsCollected.Clear();

            foreach (string line in lines)
            {
                if (int.TryParse(line, out int number))
                {
                    partsCollected.Add(number);
                }
            }
        }
        else
        {
            print("File does not exist.");
        }

        return partsCollected;
    }
}