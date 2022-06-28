using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrunkStats : MonoBehaviour
{
    // Start is called before the first frame update
    private DrunkEffect drunkEffect;
    private PlayerCombat playerCombat;
    private PlayerHealth playerHealth;
    //incomplete script as all of the drinks have not been added yet

    private void Awake()
    {
        drunkEffect = FindObjectOfType<DrunkEffect>();
        playerCombat = FindObjectOfType<PlayerCombat>();
        playerHealth = FindObjectOfType<PlayerHealth>();
       
    }    
    public void AddWineStats()
    {
       drunkEffect.UpdateStats(0.5f, false);
        playerCombat.UpdatePlayerDamage(5, 5);
        playerHealth.UpdatePlayerHealth(10);

    }

    public void AddRumStats()
    {
        drunkEffect.UpdateStats(0.7f, false);
        playerCombat.UpdatePlayerDamage(10, 10);
        playerHealth.UpdatePlayerHealth(20);
    }
    // Update is called once per frame
   
}
