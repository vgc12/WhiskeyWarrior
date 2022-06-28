using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private PlayerInputActions inputActions;
    private PlayerLook playerLook;
    
    private PlayerCombat PlayerCombat;
    
   
    

    public bool isCrouching;
    private void Awake()
    {
        
        playerLook = GameObject.FindObjectOfType<PlayerLook>();
        PlayerCombat = GameObject.FindObjectOfType<PlayerCombat>();
       
        inputActions = new PlayerInputActions();
        inputActions.Player.Control.performed += ctx => isCrouching = ctx.ReadValueAsButton();

        if (PlayerCombat != null && playerLook != null)
        {
            // Mouse input that changes float of mouseX and mouseY in PlayerLook script
            inputActions.Player.MouseX.performed += ctx => playerLook.mouseX = ctx.ReadValue<float>();
            inputActions.Player.MouseY.performed += ctx => playerLook.mouseY = ctx.ReadValue<float>();

            // changes buttonPressed in PlayerCombat script
            inputActions.Player.LeftMouse.performed += ctx => PlayerCombat.buttonPressed = 0;
            inputActions.Player.RightMouse.performed += ctx => PlayerCombat.buttonPressed = 1;
            inputActions.Player.LeftKick.performed += ctx => PlayerCombat.buttonPressed = 2;
            inputActions.Player.RightKick.performed += ctor => PlayerCombat.buttonPressed = 3;
            inputActions.Player.RoundHouse.performed += ctx => PlayerCombat.buttonPressed = 4;
        }
        
        
    }

    

    // Update is called once per frame
   

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDestroy()
    {
        inputActions.Disable();
    }

}
