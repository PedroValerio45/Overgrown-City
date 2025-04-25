using UnityEngine;

[CreateAssetMenu(fileName = "DialogueData", menuName = "ScriptableObjects/Dialogues/DialogueData", order = 1)]
public class SO_Dialogue : ScriptableObject
{
    public string[] Dialogues; //the dialogue that plays
    public DialogueOption[] DialogueOptions; //the dialogue option buttons
}