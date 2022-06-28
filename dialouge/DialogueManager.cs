using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using UnityEngine.InputSystem;
using System.Linq;
using System;
using UnityEngine.EventSystems;

public class DialogueManager : MonoBehaviour
{

    public int drinkChoice;
    public int money;
  
   
    [SerializeField] private GameObject dialougePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private GameObject moneyCounter;
    [SerializeField] private GameObject choiceContainer;



    private PlayerPrefSaving prefSaving;
    public bool spacePressed;
    [SerializeField] private GameObject handHolder;

    [SerializeField] private GameObject[] choices;
    private Animator camAnimator;

    private TextMeshProUGUI[] choicesText;

    

    private Story currentStory;




    private void Awake()
    {
        DialogueVariables.drinkChoice = -1;
        camAnimator = Camera.main.GetComponent<Animator>();
        DialogueVariables.dialogueIsPlaying = false;
       

    }

    private void Start()
    {
        prefSaving = FindObjectOfType<PlayerPrefSaving>();
        money = prefSaving.GetPlayerMoney();
        DialogueVariables.dialogueIsPlaying = false;
        dialougePanel.SetActive(false);
        choicesText = new TextMeshProUGUI[choices.Length];
        int index = 0;
        foreach(GameObject choice in choices)
        {
            choicesText[index] = choice.GetComponentInChildren<TextMeshProUGUI>();
            index++;
        }
    }

    

    public void EnterDialogueMode(TextAsset inkJSON)
    {
        camAnimator.enabled = false;
        currentStory = new Story(inkJSON.text);
        DialogueVariables.dialogueIsPlaying = true;
        dialougePanel.SetActive(true);
        handHolder.SetActive(false);
    
        FindObjectOfType<Player>().enabled = false;
        FindObjectOfType<PlayerCombat>().enabled = false;
        FindObjectOfType<PlayerLook>().enabled = false;
        Camera.main.fieldOfView = 15;

        FindObjectOfType<JumfreyDialogueStart>().enabled = false;
        FindObjectOfType<JumfreyDialogueManager>().enabled = false;
        FindObjectOfType<BillDialogueManager>().enabled = false;
        FindObjectOfType<BillDialogueStart>().enabled = false;
        FindObjectOfType<CoreyDialogueStart>().enabled = false;
        FindObjectOfType<CoreyDialogueManager>().enabled = false;
        FindObjectOfType<CalvinDialogueStart>().enabled = false;
        FindObjectOfType<CalvinDialogueManager>().enabled = false;
        FindObjectOfType<GuptaDialogueStart>().enabled = false;
        FindObjectOfType<GuptaDialogueManager>().enabled = false;
        
        DialogueVariables.currentCharater = 0;
        moneyCounter.SetActive(false);
        choiceContainer.SetActive(true);
        ContinueStory();
    }

    private void ExitDialogueMode(bool state)
    {
        moneyCounter.SetActive(true);
        camAnimator.enabled = true;
        DialogueVariables.dialogueIsPlaying = false;
        dialougePanel.SetActive(false);
        
        dialogueText.text = "";
        DialogueVariables.currentCharater = -1;


        Camera.main.fieldOfView = 90;
        choiceContainer.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        FindObjectOfType<PlayerBarInteraction>().readyToTalk = state;
    
        FindObjectOfType<Player>().enabled = true;
        FindObjectOfType<PlayerCombat>().enabled = true;
        FindObjectOfType<PlayerLook>().enabled = true;

        FindObjectOfType<JumfreyDialogueStart>().enabled = true;
        FindObjectOfType<JumfreyDialogueManager>().enabled = true;
        FindObjectOfType<BillDialogueManager>().enabled = true;
        FindObjectOfType<BillDialogueStart>().enabled = true;
        FindObjectOfType<CoreyDialogueStart>().enabled = true;
        FindObjectOfType<CoreyDialogueManager>().enabled = true;
        FindObjectOfType<CalvinDialogueStart>().enabled = true;
        FindObjectOfType<CalvinDialogueManager>().enabled = true;
        FindObjectOfType<GuptaDialogueStart>().enabled = true;
        FindObjectOfType<GuptaDialogueManager>().enabled = true;
        

    }

    // Update is called once per frame
    void Update()
    {
  
        if (!DialogueVariables.dialogueIsPlaying || DialogueVariables.currentCharater != 0) return;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ContinueStory();
            spacePressed = false;
        }

       
        
    }

    private void ContinueStory()
    {
        if (currentStory.canContinue)
        {
            
            if(currentStory.variablesState["name"] != null)
                currentStory.variablesState["name"] = Environment.UserName;
            dialogueText.text = currentStory.Continue();
            DisplayChoices();
            foreach(var tag in currentStory.currentTags)
            {
                if (PlayerPrefs.GetInt("money") >= 10)
                {
                    if (tag.StartsWith("winestats"))
                    {
                        money -= 10;
                        prefSaving.SetPlayerMoney(money);
                       DialogueVariables.drinkChoice = 0;
                    }
                    else if (tag.StartsWith("rumstats"))
                    {
                        money -= 10;
                        prefSaving.SetPlayerMoney(money);
                        DialogueVariables.drinkChoice = 1;
                    }
                    else if (tag.StartsWith("Repeat"))
                    {
                        DialogueVariables.dialogueOption = 1;
                       
                        //Debug.Log(DialogueVariables.dialogueOption + " " + FindObjectOfType<DataHelper>().money + " " + DialogueVariables.dialogueIsPlaying);
                        ExitDialogueMode(true);
                    }

                }
            }


        }
        else
        {
            ExitDialogueMode(false);
        }
    }

    private void DisplayChoices()
    {
        List<Choice> currentChoices = currentStory.currentChoices;

        currentChoices.ForEach(X => Debug.Log(X));
        if(currentChoices.Count > choices.Length)
        {
            Debug.LogError("more choices given then ui can support " + currentChoices.Count);
        }
        int index = 0;
        foreach(Choice choice in currentChoices)
        {
            choices[index].SetActive(true);
            choicesText[index].text = choice.text;
            index++;
        }

        for(int i = index; i < choices.Length; i++)
        {
            choices[i].SetActive(false);
        }
        StartCoroutine(SelectFirstChoice());
    }

    public void MakeChoice(int choiceIndex)
    {
        Debug.Log(choiceIndex);
        Debug.Log(currentStory.currentChoices[choiceIndex]);
        currentStory.ChooseChoiceIndex(choiceIndex);
        
        ContinueStory();
    }

    private IEnumerator SelectFirstChoice()
    {
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(choices[0]);
    }

    
}
