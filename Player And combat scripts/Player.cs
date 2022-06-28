using System;
using UnityEngine;
using UnityEngine.InputSystem;
public class Player : MonoBehaviour
{

    

    public Transform cameraHolder;

   

    public float playerSpeed = 12.5f;
    [Header("Animations")]

    [SerializeField] Animator PlayerWalking;


    [Header("Combat")]
    
    public float attackRate = 2f;

    public float attackRange = 0.5f;
    public LayerMask enemyLayers;



    [Header("Movement")]

    private float horizontalInput, verticalInput;
    public LayerMask groundMask;
    private Vector3 moveDirection;
    public float movementMultiplier = 10f;
    Rigidbody playerRB;
    
    public float airMultiplier = 0.4f;
    public Transform orientation;
    public CapsuleCollider capsuleCollider;


    [Header("Crouching")]

    private Vector3 crouchScale = new Vector3(1, 0.5f, 1);
    private Vector3 playerScale;
    [SerializeField] float crouchSpeed = 1f;
    private bool isCrouching;
    private PlayerInputActions inputActions;
    


    [Header("Drag")]
    public float groundRBDrag = 6f;

    


    private void Awake()
    {
        inputActions = new PlayerInputActions();
        inputActions.Player.Enable();
      
        playerRB = GetComponent<Rigidbody>();
        inputActions.Player.Control.performed += ctx => isCrouching = ctx.ReadValueAsButton();

        
    }

   
    private void FixedUpdate()
    {
        
        
        MovePlayer();
        Crouching();
    }


    


    // Start is called before the first frame update
    void Start()
    {
        playerRB.freezeRotation = true;
        
        playerScale = transform.localScale;

    }

    // Update is called once per frame
    void Update()
    {
       

        PlayerWalking.SetFloat("walking", playerRB.velocity.magnitude);

        MyInput();

        ControlDrag();
        
      
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        moveDirection = transform.forward * verticalInput + transform.right * horizontalInput;
        
    }

    private void MovePlayer()
    {
        if(!isCrouching)
            playerRB.AddForce(movementMultiplier * playerSpeed * moveDirection.normalized, ForceMode.Acceleration);
        if (isCrouching)
            playerRB.AddForce(crouchSpeed * movementMultiplier * moveDirection.normalized, ForceMode.Acceleration);
    }

    private void Crouching()
    {
        if (isCrouching)
        {
            StartCrouching();
        }
        if (!isCrouching)
        {
            StopCrouching();
        }
    }


    void StartCrouching()
    {
        
        transform.localScale = crouchScale;


    }
    void StopCrouching()
    {
        
        transform.localScale = playerScale;

    }

    void ControlDrag()
    {
        
       playerRB.drag = groundRBDrag;
       
    }

}