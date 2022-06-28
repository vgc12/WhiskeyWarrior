using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumfreyDialogueStart : MonoBehaviour
{
    [Header("Visual CUe")]
    [SerializeField] private GameObject uiElement;


    [SerializeField] private Transform player;
    public Transform jumfrey;
    private LayerMask playerMask;

    [Header("Ink JSON")]
    [SerializeField] private TextAsset jumfreyText;


    private void Start()
    {

       
        playerMask = ~LayerMask.GetMask("Player", "Ignore Raycast");
    }

    private void LateUpdate()
    {
        
        Collider[] colliders = Physics.OverlapSphere(transform.position, 3f, playerMask);
        if (!DialogueVariables.dialogueIsPlaying)
        {
            foreach (Collider col in colliders)
            {

                if (col.CompareTag("jumfrey") || col.CompareTag("jumfreyHead"))
                {
                    uiElement.SetActive(true);
                    
                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        GetComponent<JumfreyDialogueManager>().EnterJumfreyDialogueMode(jumfreyText);
                        uiElement.SetActive(false);
                    }
                }

            }
        }
        else if (DialogueVariables.dialogueIsPlaying)
        {
            uiElement.SetActive(false);
            StartCoroutine(RotateToJumfrey());
        }
        else
        {
            uiElement.SetActive(false);
        }
       
    }
    private IEnumerator RotateToJumfrey()
    {
        float t = 0;
        while (true)
        {
            while (t < 1)
            {

                t += Time.deltaTime * .3f;
                player.rotation = Quaternion.Slerp(player.rotation, Quaternion.LookRotation(GameObject.FindGameObjectWithTag("jumfreyHead").transform.position - player.position), t);
                Vector3 newRotation = new Vector3(player.position.x - jumfrey.position.x, 0.0f, player.position.z - jumfrey.position.z);
                Quaternion target = Quaternion.LookRotation(newRotation);
                jumfrey.rotation = Quaternion.Slerp(player.transform.rotation, target, t);

                player.LookAt(GameObject.FindGameObjectWithTag("jumfreyHead").transform.position);
            }
            yield return null;
        }
    }
}
