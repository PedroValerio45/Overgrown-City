using UnityEngine;


[CreateAssetMenu(fileName = "DialogueData", menuName = "ScriptableObjects/Dialogues/DialogueData", order = 1)]
public class SO_Dialogue : ScriptableObject
{
    public string[] CharacterName;
    public string[] Dialogues;
    public DialogueOption[] DialogueOptions;
}