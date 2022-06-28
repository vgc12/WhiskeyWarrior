using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialougeTrigger : MonoBehaviour
{
    [Header("Visual CUe")]
    [SerializeField] private GameObject uiElement, trigger;
    private bool playerInRange;
    [SerializeField] private Transform npcHead;
    [SerializeField] private Transform player;
    public int dialougeOption;

    [Header("Ink JSON")]
    [SerializeField] private TextAsset introDialogue;
    [SerializeField] private TextAsset secondDialogue, noMoneyDialogue;
    private DialogueManager dialogueManager;
    private PlayerBarInteraction playerBarInteraction;
  
    [SerializeField] private AudioSource growlSoundEffect;
    
    void Start()
    {
        playerInRange = false;
        uiElement.SetActive(false);
        dialogueManager = FindObjectOfType<DialogueManager>();
       
        playerBarInteraction = FindObjectOfType<PlayerBarInteraction>();
    }

    private void OnTriggerEnter(Collider other)
    {
        dialogueManager.enabled = true;
        uiElement.SetActive(true);
        if (other.CompareTag("Player"))
            playerInRange = true;
        

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            uiElement.SetActive(false);
            dialogueManager.enabled = false;
            if (other.CompareTag("Player"))
                playerInRange = false;
            
        }
    }

    // Update is called once per frame
    void Update()
    {


        if (playerBarInteraction.readyToTalk && 
            playerInRange && dialogueManager.enabled == true && 
            !DialogueVariables.dialogueIsPlaying)
        {
            if(dialogueManager.money < 10)
            {
                uiElement.SetActive(true);
                if (Input.GetKeyDown(KeyCode.F))
                {
                   
                    
                    growlSoundEffect.Play();
                   
                    dialogueManager.EnterDialogueMode(noMoneyDialogue);
                    uiElement.SetActive(false);
                }
            }
            else if (DialogueVariables.dialogueOption == 0)
            {

                uiElement.SetActive(true);
                if (Input.GetKeyDown(KeyCode.F))
                {

                    dialogueManager.EnterDialogueMode(introDialogue);
                    uiElement.SetActive(false);
                }
            }
            else if(DialogueVariables.dialogueOption == 1
                    || dialogueManager.money > 10)
            {
                uiElement.SetActive(true);
               
                if (Input.GetKeyDown(KeyCode.F))
                {
              
                    dialogueManager.EnterDialogueMode(secondDialogue);
                    uiElement.SetActive(false);
                }
            }
            else
            {
                uiElement.SetActive(false);
                StartCoroutine(RotateCam());
            }
            
        }
        else if (DialogueVariables.dialogueIsPlaying)
        {
            uiElement.SetActive(false);
            StartCoroutine(RotateCam());
        }
        else
        {
            uiElement.SetActive(false); 
        }
    }
    



    // anytime there is a Rotate() courotine its to rotate the players head to the position of the npc being talked to.
    IEnumerator RotateCam()
    {
        float t = 0;
        while (true){
            while (t < 1)
            {
                t += Time.deltaTime * .3f;
                player.rotation = Quaternion.Slerp(player.rotation, Quaternion.LookRotation(npcHead.position - player.position), t);
                
                player.LookAt(npcHead);
            }
            yield return null;
        }
    }
}

