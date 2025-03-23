using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor.Experimental;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    // Stuff in this script is to be saved between scenes

    // Amount of boat parts collected (will prob go unused)
    public static int partsCollectedAmount = 0;
    
    // List of boat parts collected
    public static List<int> partsCollected = new List<int>();
    
    public void CreateOrWritePlayerFile()
    {
        string filePath = Path.Combine(Application.dataPath, "playerData.txt");

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
    
    public List<int> ReadPlayerFile()
    {
        // string fileName = "PlayerData.txt";
        string filePath = Path.Combine(Application.dataPath, "playerData.txt");

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