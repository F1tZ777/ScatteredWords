using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class DialogueSet
{
    [TextArea]
    public List<string> lines;
    public bool showButton;
}

public class Dialogue : MonoBehaviour
{
    public List<DialogueSet> DialogueLines;
    public GameObject ButtonsMinigame;
    public TMP_Text textBox;
    public GameObject startButton;

    string[] queuedLines;
    int lineCount = 0;
    bool isTalking = false;
    bool willShowButton = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isTalking && Input.GetMouseButtonDown(0))
        {
            NextLine();
        }
    }

    public void StartDialogue(int ID)
    {
        startButton.SetActive(false);
        isTalking = true;
        // 1. Get n-th item of DialogueLines
        DialogueSet currentSet = DialogueLines[ID];
        // 2. Get lines in said item and fill it into queuedLines
        queuedLines = currentSet.lines.ToArray();
        // 3. Get the bool to show the buttons at the end of dialogue
        willShowButton = currentSet.showButton;
        // 4. Reset line counter
        lineCount = 0;
        // 5. Run NextLine function to start the first line
        NextLine();
    }

    void NextLine()
    {
        if (lineCount < queuedLines.Length)
        {
            textBox.text = queuedLines[lineCount];
            lineCount++;
        }

        else
        {
            textBox.text = string.Empty;
            isTalking = false;
            if (willShowButton)
            {
                ButtonsMinigame.SetActive(true);
            }
        }
    }
}
