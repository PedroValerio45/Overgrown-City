using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PromptE : MonoBehaviour
{
    // TARGET HAS TO BE SET MANUALLY IN EVERY OBJECT IN EVERY SCENE (FreeLook Camera prefab)
    // This is present in the PlayerNEW prefab
    public Transform target;
    public MeshRenderer promptEImage;
    public Material pressedYes;
    public Material pressedNo;
    void Update()
    {
        if (target != null) { transform.LookAt(target); }

        if (Input.GetKey(KeyCode.E)) { promptEImage.material =  pressedYes; }
        else { promptEImage.material =   pressedNo; }
    }
}