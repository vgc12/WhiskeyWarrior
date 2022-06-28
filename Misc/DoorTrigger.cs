using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    [SerializeField] private Animator doorAnimator;
    


    private void OnTriggerEnter(Collider other)
    {
        doorAnimator.SetBool("open", true);
       
    }
    private void OnTriggerExit(Collider other)
    {
        doorAnimator.SetBool("open", false);
        
    }
}
