using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CoreyDialogueStart : MonoBehaviour
{
    [Header("Visual CUe")]
    [SerializeField] private GameObject uiElement;

    [SerializeField] private Transform otherNPCHEAD;
    [SerializeField] private Transform player;
    public NavMeshAgent coreyAgent;
    private LayerMask playerMask;



    [Header("Ink JSON")]
    [SerializeField] private TextAsset calvinText;


    private void Start()
    {

        uiElement.SetActive(false);
        playerMask = ~LayerMask.GetMask("Player", "Ignore Raycast");
    }

    // Update is called once per frame
    private void Update()
    {

        Collider[] colliders = Physics.OverlapSphere(transform.position, 3f, playerMask);



        if (!DialogueVariables.dialogueIsPlaying)
        {
            foreach (Collider col in colliders)
            {

                if (col.CompareTag("corey"))
                {
                    


                    uiElement.SetActive(true);
                    if (Input.GetKeyDown(KeyCode.F))
                    {
                      
                        

                        GetComponent<CoreyDialogueManager>().EnterCoreyDialogueMode(calvinText);
                        uiElement.SetActive(false);
                    }
                }

            }
        }
        else if (DialogueVariables.dialogueIsPlaying)
        {
            uiElement.SetActive(false);
            StartCoroutine(RotateToCorey());
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

    private IEnumerator RotateToCorey()
    {
        float t = 0;
        while (true)
        {
            while (t < 1)
            {

                t += Time.deltaTime * .3f;
                player.rotation = Quaternion.Slerp(player.rotation, Quaternion.LookRotation(GameObject.FindGameObjectWithTag("coreyHead").transform.position - player.position), t);
                Vector3 newRotation = new Vector3(player.position.x - coreyAgent.transform.position.x, 0.0f, player.position.z - coreyAgent.transform.position.z);
                Quaternion target = Quaternion.LookRotation(newRotation);
                coreyAgent.transform.rotation = Quaternion.Slerp(player.transform.rotation, target, t);

                player.LookAt(GameObject.FindGameObjectWithTag("coreyHead").transform.position);
            }
            yield return null;
        }
    }
}
