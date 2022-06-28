using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FrontDoorTrigger : MonoBehaviour
{
    [SerializeField] private Animator[] doorAnimators;

    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("gupta") || other.CompareTag("Player") || other.CompareTag("guptaHead") && DialogueVariables.drinkConsumed == true)
            doorAnimators.ToList().ForEach(x => x.SetBool("open", true));
    }

    private void OnTriggerExit(Collider other)
    {
        doorAnimators.ToList().ForEach(x => x.SetBool("open", false));
    }

}
