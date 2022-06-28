using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(CalvinDialogueManager))]
public class CalvinDialogueStart : MonoBehaviour
{

    [Header("Visual CUe")]
    [SerializeField] private GameObject uiElement;

    [SerializeField] private Transform otherNPCHEAD;
    [SerializeField] private Transform player;
    public NavMeshAgent calvinAgent;
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
             
                if (col.CompareTag("calvin"))
                {
                    Debug.Log(col + " tag : " + col.tag);


                    uiElement.SetActive(true);
                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        

                        GetComponent<CalvinDialogueManager>().EnterCalvinDialogueMode(calvinText);
                        uiElement.SetActive(false);
                    }
                }

            }
        }
        else if (DialogueVariables.dialogueIsPlaying)
        {
            uiElement.SetActive(false);
            StartCoroutine(RotateToCalvin());
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

    private IEnumerator RotateToCalvin()
    {
        float t = 0;
        while (true)
        {
            while (t < 1)
            {

                t += Time.deltaTime * .3f;
                player.rotation = Quaternion.Slerp(player.rotation, Quaternion.LookRotation(GameObject.FindGameObjectWithTag("calvinHead").transform.position - player.position), t);
                Vector3 newRotation = new Vector3(player.position.x - calvinAgent.transform.position.x, 0.0f, player.position.z - calvinAgent.transform.position.z);
                Quaternion target = Quaternion.LookRotation(newRotation);
                calvinAgent.transform.rotation = Quaternion.Slerp(player.transform.rotation, target, t);

                player.LookAt(GameObject.FindGameObjectWithTag("calvinHead").transform.position);
            }
            yield return null;
        }
    }
}
