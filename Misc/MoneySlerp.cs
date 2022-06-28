using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MoneySlerp : MonoBehaviour
{


   

    // Update is called once per frame
    void Update()
    {
        // rotates money 50 degrees on the local z axis every second
        // this could probably use a better name as i am no longer using slerp to rotate it as it is not necassary.
        transform.Rotate(0, 0, 50 * Time.deltaTime);


    }
}
