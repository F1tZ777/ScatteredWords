using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ButtonSequence
{
    public List<int> sequence;
}

public class ButtonHandler : MonoBehaviour
{
    //public List<ButtonSequence> buttonSequence;
    public List<int> buttonSequence;
    private List<int> currentInput = new List<int>();
    public Dialogue dialogueSystem; // Reference to Dialogue.cs
    //public DialogueTrigger dialogueTrigger; // Reference to DialogueTrigger.cs
    public int correctDialogueID, incorrectDialogueID;

    public void RegisterButtonPress(int id)
    {
        currentInput.Add(id);

        // Compare lengths to see if the sequence is done
        if (currentInput.Count == buttonSequence.Count)
        {
            bool success = true;
            for (int i = 0; i < currentInput.Count; i++)
            {
                if (currentInput[i] != buttonSequence[i])
                {
                    success = false;
                    break;
                }
            }

            if (success)
            {
                dialogueSystem.StartDialogue(correctDialogueID); // e.g., success dialogue
                //dialogueTrigger.OnCorrectSequence();
            }
            else
            {
                dialogueSystem.StartDialogue(incorrectDialogueID); // e.g., failure dialogue
                //dialogueTrigger.OnIncorrectSequence();
            }

            currentInput.Clear(); // Reset for retry or next round
        }
    }
}
