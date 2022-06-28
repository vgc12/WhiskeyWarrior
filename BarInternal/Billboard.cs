using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Transform mCamera;



    // Start is called before the first frame update
    void Start()
    {
        mCamera = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 target = transform.position - mCamera.position;
        transform.rotation = Quaternion.LookRotation(target, mCamera.rotation * Vector3.up);
    }
}
