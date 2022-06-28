using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class GuptaDialogueStart : MonoBehaviour
{
    [Header("Visual CUe")]
    [SerializeField] private GameObject uiElement;

    [SerializeField] private Transform otherNPCHEAD;
    [SerializeField] private Transform player;
    public NavMeshAgent guptaAgent;
    private LayerMask playerMask;
    
    [SerializeField] private GuptaDialogueManager dialogueManager;

    [Header("Ink JSON")]
    [SerializeField] private TextAsset unibomberDialogue;
    

    private void Start()
    {
       
        uiElement.SetActive(false);
        playerMask = ~LayerMask.GetMask("Player","Ignore Raycast");
    }

    // Update is called once per frame
    private void Update()
    {

        Collider[] colliders = GetComponent<PlayerBarInteraction>().overlapCols;



        if (!DialogueVariables.dialogueIsPlaying)
        {
            foreach (Collider col in colliders)
            {
                
                if (col.CompareTag("gupta"))
                {

                    uiElement.SetActive(true);
                    if (Input.GetKeyDown(KeyCode.F))
                    {
                       
                        
                        GetComponent<GuptaDialogueManager>().EnterGuptaDialogueMode(unibomberDialogue);
                        uiElement.SetActive(false);
                    }
                }
                
            }
        }
        else if (DialogueVariables.dialogueIsPlaying)
        {
            
            uiElement.SetActive(false);
            StartCoroutine(RotateToGupta());
        }
        else
        {
            uiElement.SetActive(false);
        }



    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Debug.DrawRay(transform.position, transform.forward);
        
    }

    private IEnumerator RotateToGupta()
    {
        float t = 0;
        while (true)
        {
            while (t < 1)
            {

                t += Time.deltaTime * .3f;
                player.rotation = Quaternion.Slerp(player.rotation, Quaternion.LookRotation(GameObject.FindGameObjectWithTag("guptaHead").transform.position - player.position), t);
                Vector3 newRotation = new Vector3(player.position.x - guptaAgent.transform.position.x, 0.0f, player.position.z - guptaAgent.transform.position.z);
                Quaternion target = Quaternion.LookRotation(newRotation);
                guptaAgent.transform.rotation = Quaternion.Slerp(player.transform.rotation, target, t);

                player.LookAt(GameObject.FindGameObjectWithTag("guptaHead").transform.position);
            }
            yield return null;
        }
    }
}
