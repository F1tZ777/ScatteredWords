using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InkDialogueSet
{
    [SerializeField]TextAsset inkJSON;
    [SerializeField] bool showButton;
    [SerializeField] bool mergeBranch;
    [SerializeField] int mergedID;
    // Preparing in case one dialogue sequence with a character requires multiple buttonsMinigame
    // Remember to edit the editor script
    //[SerializeField] GameObject buttonsMinigame;
}

public class DialogueTrigger : MonoBehaviour
{
    [Header("Visual Cue")]
    [SerializeField] GameObject dialogueIcon;

    [Header("Ink JSON")]
    //[SerializeField] TextAsset inkJSON;
    [SerializeField] public List<InkDialogueSet> dialogueSets;

    public GameObject buttonsMinigame;

    bool playerInRange;

    void Awake()
    {
        playerInRange = false;
        dialogueIcon.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInRange && !DialogueManager.GetInstance().dialogueIsPlaying)
        {
            dialogueIcon.SetActive (true);
            if (Input.GetKeyDown(KeyCode.E)) // Change this when we make input manager
            {
                // Dialogue stuff here (DO NOT DO ANYTHING, KEEP THIS AS IS FOR NOW)
            }
        }

        else
        {
            dialogueIcon.SetActive (false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            playerInRange = false;
        }
    }

    void StartDialogue()
    {
        //DialogueManager.GetInstance().EnterDialogueMode(dialogueSets[0].inkJSON, buttonsMinigame);
    }

    public void OnCorrectSequence()
    {
        //DialogueManager.GetInstance().EnterDialogueMode(dialogueSets[1].inkJSON);
    }

    public void OnIncorrectSequence() 
    {
        //DialogueManager.GetInstance().EnterDialogueMode(dialogueSets[2].inkJSON);
    }
}
