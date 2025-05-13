using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class promptE : MonoBehaviour
{
    //The E prompt is controlled by the Collectable, ClimbObject, NPC_Behaviour, and Door scripts
    
    public Image prompt;
    public Sprite pressed;
    public Sprite notPressed;
    
    void Start() { PromptE_Disable(); }

    void Update()
    {
        if (Input.GetKey(KeyCode.E)) { prompt.sprite = pressed; }
        else { prompt.sprite = notPressed; }
    }

    public void PromptE_Show() { prompt.enabled = true; }
    
    public void PromptE_Disable() { prompt.enabled = false; }
}
