using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int playerHealth;
    public int maxHealth;
    [SerializeField] private GameObject handHolder;
    [SerializeField] private GameObject deadScreen;
    private PlayerPrefSaving prefSaving;



    // Start is called before the first frame update
    void Start()
    {
        prefSaving = FindObjectOfType<PlayerPrefSaving>();
        maxHealth = prefSaving.GetPlayerHealth();
        playerHealth = maxHealth;
        
    }

    public void UpdatePlayerHealth(int health)
    {
        prefSaving.SetPlayerHealth(maxHealth + health);
        maxHealth = prefSaving.GetPlayerHealth();
        playerHealth = maxHealth;
        
    }
   
    public void TakeDamage(int damage)
    {
        playerHealth -= damage;

        if (playerHealth <= 0)
            Die();
    }

    private void Die()
    {
        
        FindObjectOfType<PlayerCombat>().enabled = false;
       
        FindObjectOfType<PlayerLook>().enabled = false;
        FindObjectOfType<Player>().enabled = false;
        FindObjectOfType<GameManager>().gameOver = true;
        FindObjectOfType<GameManager>().UnlockCursor();
        
        Destroy(GameObject.FindGameObjectWithTag("Enemy"));

        handHolder.SetActive(false);
        deadScreen.SetActive(true);
        
    }
}
