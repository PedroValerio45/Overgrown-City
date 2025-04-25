using UnityEngine;

[CreateAssetMenu(fileName = "DialogueOption", menuName = "ScriptableObjects/Dialogues/DialogueOption", order = 1)]
public class DialogueOption : ScriptableObject
{
    public string DialogueOptionText; //text in the button
    public SO_Dialogue[] NextDialoguePossibilities; //what the next dialogue piece after clicking the button is, based on values on the player's stats
    public int optionID;
}