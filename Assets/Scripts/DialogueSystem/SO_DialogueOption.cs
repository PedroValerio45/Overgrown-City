using UnityEngine;

[CreateAssetMenu(fileName = "DialogueOption", menuName = "ScriptableObjects/Dialogues/DialogueOption", order = 1)]
public class DialogueOption : ScriptableObject
{
    public string DialogueOptionText;
    public SO_Dialogue NextDialoguePossibilities;
}