using UnityEngine;

public class PlayerLook : MonoBehaviour
{

    [Header("MouseInput")]
    public float mouseX;
    public float mouseY;
    float multiplier = 2f;
    float xRotation, yRotation;
    [SerializeField] private float sensX = 100f;
    public float sensY = 100f;
    [SerializeField] private Transform cam;
    [SerializeField] private Transform orientation;
    [SerializeField] private Transform model;
    
    
    


   


    private void Update()
    {
        //
        yRotation += mouseX * sensX * multiplier * Time.fixedDeltaTime;
        xRotation -= mouseY * sensY * multiplier * Time.fixedDeltaTime;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cam.transform.rotation = Quaternion.Euler(xRotation, yRotation, 0); ;
        orientation.transform.rotation = Quaternion.Euler(0, yRotation, 0); ;

        model.transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }


    private void OnDestroy()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
