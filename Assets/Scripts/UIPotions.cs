using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIPotions : MonoBehaviour
{
    public TMP_Text counter;
    
    public void SetCounterText(int text)
    {
        counter.text = text.ToString();
    }
}
