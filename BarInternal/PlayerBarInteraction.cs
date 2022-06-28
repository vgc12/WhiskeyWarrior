using TMPro;
using UnityEngine;

public class PlayerBarInteraction : MonoBehaviour
{

    public RaycastHit hit, secondHit;

    private LayerMask playerMask;
    [SerializeField] private TextMeshPro InfoText;
    [SerializeField] private GameObject textHolder, wineBottle, rumBottle, fakeDrinkText, bloodEffect;
    
    public Collider[] overlapCols;
    private PlayerPrefSaving prefSaving;

    private Dead dead;
    

    public bool readyToTalk;


    // Start is called before the first frame update
    private void Start()
    {
        playerMask = ~LayerMask.GetMask("Player", "Ignore Raycast");
        prefSaving = FindObjectOfType<PlayerPrefSaving>();
        dead = GetComponent<Dead>();
        readyToTalk = true;
        bloodEffect.SetActive(false);   
    }

    // Update is called once per frame
    private void Update()
    {

        overlapCols = Physics.OverlapSphere(transform.position, 3f, playerMask);

        if (Physics.Raycast(transform.position, transform.forward, out hit, 5f, playerMask) && !DialogueVariables.dialogueIsPlaying && !readyToTalk)
        {
      
            if (DialogueVariables.drinkChoice == 0)
            {
                
                wineBottle.SetActive(true);
                if (hit.collider.CompareTag("wine"))
                {
                    textHolder.SetActive(true);
                    InfoText.gameObject.SetActive(true);
                    InfoText.text = "Le Miserable Tante - 20% ABV: +15 max FOV, -15 min FOV, + 5 overall damage, + 10 max health";
                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        DialogueVariables.amountDrunk++;
                        prefSaving.SetDrinksConsumed(DialogueVariables.amountDrunk);
                        FindObjectOfType<DrunkStats>().AddWineStats();
                        wineBottle.SetActive(false);
                        textHolder.SetActive(false);
                        DialogueVariables.drinkChoice = -1;
                        DialogueVariables.dialogueOption++;
                        readyToTalk = true;
                        DialogueVariables.drinkConsumed = true;
                    
                    }
                }
                else
                {
                    InfoText.gameObject.SetActive(false);
                    textHolder.SetActive(false);
                }
            }
            if (DialogueVariables.drinkChoice == 1)
            {
                rumBottle.SetActive(true);
                if (hit.collider.CompareTag("rum"))
                {
                    textHolder.SetActive(true);
                    InfoText.gameObject.SetActive(true);
                    InfoText.text = "Hit & Run Rum- 40% ABV: +10 max FOV, -10 min FOV, + 10 overall damage, + 20 max health";
                    if (Input.GetKeyDown(KeyCode.F))
                    {
                     
                        DialogueVariables.amountDrunk++;
                        prefSaving.SetDrinksConsumed(DialogueVariables.amountDrunk);
                        FindObjectOfType<DrunkStats>().AddRumStats();
                        rumBottle.SetActive(false);
                        textHolder.SetActive(false);
                        DialogueVariables.drinkChoice = -1;
                        DialogueVariables.dialogueOption++;
                        readyToTalk = true;
                        DialogueVariables.drinkConsumed = true;
                    }
                }
                else
                {
                    InfoText.gameObject.SetActive(false);
                    textHolder.SetActive(false);
                }
            }

            if (hit.collider == null)
            {
                textHolder.SetActive(false);
                textHolder.SetActive(false);
                return;
            }
        }

        if (Physics.Raycast(transform.position, transform.forward, out secondHit, 2f, playerMask))
        {
            if (secondHit.collider.CompareTag("dontDrink"))
            {
                fakeDrinkText.SetActive(true);
                if (Input.GetKeyDown(KeyCode.F))
                {
                    bloodEffect.SetActive(true);
                    dead.Die();
                }
            }
            else
            {
                fakeDrinkText.SetActive(false);
            }
        }

       if (secondHit.collider == null)
            fakeDrinkText.SetActive(false);

    }
   
}
