using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using UnityEngine.EventSystems;

public class DialogueManager : MonoBehaviour
{
    [Header("Dialogue UI")]
    [SerializeField] GameObject dialogueBox;
    [SerializeField] TMP_Text dialogueText;

    [Header("Choices UI")]
    [SerializeField] GameObject[] choices;
    TMP_Text[] choicesText;

    Story currentStory;

    GameObject buttonsMinigame;

    public bool dialogueIsPlaying { get; private set; }

    static DialogueManager instance;

    void Awake()
    {
        if (instance == null)
            instance = this;
    }

    void Start()
    {
        dialogueIsPlaying = false;
        dialogueBox.SetActive(false);

        // get all of the choices text
        choicesText = new TMP_Text[choices.Length];
        int index = 0;
        foreach (GameObject choice in choices)
        {
            choicesText[index] = choice.GetComponentInChildren<TMP_Text>();
            index++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!dialogueIsPlaying)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            ContinueStory();
        }

    }

    public static DialogueManager GetInstance() { return instance; }

    public void EnterDialogueMode(TextAsset inkJSON, GameObject buttons = null, bool mergeBranch = false, int mergedID = -1)
    {
        buttonsMinigame = null;
        currentStory = new Story(inkJSON.text);
        dialogueIsPlaying = true;
        dialogueBox.SetActive(true);

        if (buttons != null)
            buttonsMinigame = buttons;

        ContinueStory();
    }

    void ContinueStory()
    {
        if (currentStory.canContinue)
        {
            dialogueText.text = currentStory.Continue();
            //DisplayChoices(); // reenable this when we start making choices for dialogue
        }
        else
        {
            StartCoroutine(ExitDialogueMode());
        }
    }

    void DisplayChoices()
    {
        List<Choice> currentChoices = currentStory.currentChoices;

        // defensive check to make sure our UI can support the number of choices coming in
        if (currentChoices.Count > choices.Length)
        {
            Debug.LogError("More choices were given than the UI can support!");
        }

        int index = 0;
        // enable and initialize the choices
        foreach (Choice choice in currentChoices)
        {
            choices[index].gameObject.SetActive(true);
            choicesText[index].text = choice.text;
            index++;
        }

        // go through the remaining choices the UI supports and make sure they're hidden
        for (int i = index; i < choices.Length; i++)
        {
            choices[i].gameObject.SetActive(false);
        }

        StartCoroutine(SelectFirstChoice());
    }

    IEnumerator SelectFirstChoice()
    {
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(choices[0].gameObject);
    }

    public void MakeChoice(int choiceIndex)
    {
        currentStory.ChooseChoiceIndex(choiceIndex);
    }

    IEnumerator ExitDialogueMode()
    {
        yield return new WaitForSeconds(0.1f);
        dialogueIsPlaying = false;
        dialogueBox.SetActive(false);
        dialogueText.text = "";

        if (buttonsMinigame != null)
        {
            buttonsMinigame.SetActive(true);
        }
    }
}
