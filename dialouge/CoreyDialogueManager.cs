using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using UnityEngine.InputSystem;
using System.Linq;
using System;
using UnityEngine.EventSystems;
using UnityEngine.AI;

public class CoreyDialogueManager : MonoBehaviour
{

    [Header("Player Parts")]
    [SerializeField] private Animator playerAnimator;


    [Header("Dialogue Components")]
    [SerializeField] private GameObject dialougePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private GameObject[] choices;
    [SerializeField] private GameObject scrollObject;
    [SerializeField] private GameObject moneyCounter;

    private Story currentStory;
    private TextMeshProUGUI[] choicesText;





    [Header("calvin components")]
    [SerializeField] private BarAI barAI;
    [SerializeField] private NavMeshAgent coreyAgent;
    [SerializeField] private Animator coreyAnimator;

    private void Start()
    {
        DialogueVariables.dialogueIsPlaying = false;
        dialougePanel.SetActive(false);
        choicesText = new TextMeshProUGUI[choices.Length];
        int index = 0;
        foreach (GameObject choice in choices)
        {

            choicesText[index] = choice.GetComponentInChildren<TextMeshProUGUI>();
            index++;
        }
    }



    public void EnterCoreyDialogueMode(TextAsset inkJSON)
    {
        currentStory = new Story(inkJSON.text);
        DialogueVariables.dialogueIsPlaying = true;
        dialougePanel.SetActive(true);

        FindObjectOfType<Player>().enabled = false;
        FindObjectOfType<PlayerCombat>().enabled = false;
        FindObjectOfType<PlayerLook>().enabled = false;
        barAI.enabled = false;
        coreyAgent.enabled = false;
        DialogueVariables.currentCharater = 2;
        playerAnimator.enabled = false;
        scrollObject.SetActive(true);
        moneyCounter.SetActive(false);

        Camera.main.fieldOfView = 40;

        FindObjectOfType<JumfreyDialogueStart>().enabled = false;
        FindObjectOfType<JumfreyDialogueManager>().enabled = false;
        FindObjectOfType<BillDialogueManager>().enabled = false;
        FindObjectOfType<BillDialogueStart>().enabled = false;
        FindObjectOfType<CalvinDialogueManager>().enabled = false;
        FindObjectOfType<CalvinDialogueStart>().enabled = false;
        FindObjectOfType<DialogueManager>().enabled = false;
        FindObjectOfType<DialougeTrigger>().enabled = false;
        FindObjectOfType<GuptaDialogueManager>().enabled = false;
        FindObjectOfType<GuptaDialogueStart>().enabled = false;
        FindObjectOfType<DialougeTrigger>().enabled = false;

        ContinueCoreyStory();
    }

    private void ExitCoreyDialogueMode()
    {
        dialougePanel.SetActive(false);

        dialogueText.text = "";

        DialogueVariables.currentCharater = -1;
        DialogueVariables.dialogueIsPlaying = false;
      
        FindObjectOfType<Player>().enabled = true;
        FindObjectOfType<PlayerCombat>().enabled = true;
        FindObjectOfType<PlayerLook>().enabled = true;
        FindObjectOfType<CalvinDialogueStart>().calvinAgent.isStopped = false;

        Camera.main.fieldOfView = 90;

        FindObjectOfType<JumfreyDialogueStart>().enabled = true;
        FindObjectOfType<JumfreyDialogueManager>().enabled = true;
        FindObjectOfType<BillDialogueManager>().enabled = true;
        FindObjectOfType<BillDialogueStart>().enabled = true;
        FindObjectOfType<DialougeTrigger>().enabled = true;
        FindObjectOfType<CalvinDialogueStart>().enabled = true;
        FindObjectOfType<CalvinDialogueManager>().enabled = true;
        FindObjectOfType<DialogueManager>().enabled = true;
        FindObjectOfType<GuptaDialogueManager>().enabled = true;
        FindObjectOfType<GuptaDialogueStart>().enabled = true;
        FindObjectOfType<DialougeTrigger>().enabled = true;


        barAI.enabled = true;
        coreyAgent.enabled = true;
        playerAnimator.enabled = true;
        moneyCounter.SetActive(false);
        scrollObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!DialogueVariables.dialogueIsPlaying || DialogueVariables.currentCharater != 2) return;
        coreyAnimator.SetFloat("walk", 0);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ContinueCoreyStory();

        }



    }

    private void ContinueCoreyStory()
    {
        if (currentStory.canContinue)
        {



            dialogueText.text = currentStory.Continue();
            DisplayCoreyChoices();
            /*
            foreach (var tag in currentStory.currentTags)
            {
                if (tag == "open")
                {
                    frontDoorTrigger.SetActive(true);
                }
            }
            */
        }
        else
        {
            ExitCoreyDialogueMode();
        }
    }

    private void DisplayCoreyChoices()
    {
        List<Choice> currentChoices = currentStory.currentChoices;

        currentChoices.ForEach(X => Debug.Log(X));
        if (currentChoices.Count > choices.Length)
        {
            Debug.LogError("more choices given then ui can support " + currentChoices.Count);
        }
        int index = 0;
        foreach (Choice choice in currentChoices)
        {
            choices[index].SetActive(true);
            choicesText[index].text = choice.text;
            index++;
        }

        for (int i = index; i < choices.Length; i++)
        {
            choices[i].SetActive(false);
        }
        StartCoroutine(SelectFirstChoice());
    }

    public void MakeCoreyhoice(int choiceIndex)
    {
        Debug.Log(choiceIndex);
        Debug.Log(currentStory.currentChoices[choiceIndex]);
        currentStory.ChooseChoiceIndex(choiceIndex);

        ContinueCoreyStory();
    }

    private IEnumerator SelectFirstChoice()
    {
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(choices[0]);
    }
}
