using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerPrefSaving : MonoBehaviour
{
    private DialogueManager dialogueManager;



    // terrible unencrypted saving system.
    // accessable from anywhere needed.

    private void Awake()
    {
        if(FindObjectOfType<DialogueManager>() != null)
        {
            dialogueManager = FindObjectOfType<DialogueManager>();

            dialogueManager.money = GetPlayerMoney();


            DialogueVariables.amountDrunk = GetDrinksConsumed();
        }
    }

    public int GetPrefsScene()
    {
        return PlayerPrefs.GetInt("Scene");
    }

    public void SetPrefsScene(int SceneNum)
    {
        PlayerPrefs.SetInt("Scene", SceneNum);
    }


    public int GetPlayerMoney() {
        return PlayerPrefs.GetInt("money");
    }

    public void SetPlayerMoney(int newValue)
    {
        PlayerPrefs.SetInt("money", newValue);
    }

    public int GetPlayerHealth()
    {
        
        return PlayerPrefs.GetInt("maxHealth");
    }

    public void SetPlayerHealth(int maxHealth)
    {
        PlayerPrefs.SetInt("maxHealth", maxHealth);
    }

    public int GetMaxFOV()
    {
        return PlayerPrefs.GetInt("maxFOV");
    }

    public void SetMaxFov(int maxFOV)
    {
        PlayerPrefs.SetInt("maxFOV", maxFOV);
    }

    public int GetMinFOV()
    {
        return PlayerPrefs.GetInt("minFOV");
    }

    public void SetMinFOV(int minFOV)
    {
        PlayerPrefs.SetInt("minFOV", minFOV);
    }

    
    public int GetPunchDamage()
    {
        return PlayerPrefs.GetInt("punchDamage");
    }

    public void SetPunchDamage(int punchDamage)
    {
        PlayerPrefs.SetInt("punchDamage", punchDamage);
    }

    public int GetKickDamage()
    {
        return PlayerPrefs.GetInt("kickDamage");
    }

    public void SetKickDamage(int kickDamage)
    {
        PlayerPrefs.SetInt("kickDamage", kickDamage);
    }

    public void SetDrinksConsumed(int drinksConsumed) {
        PlayerPrefs.SetInt("drinksConsumed", drinksConsumed);
    }

    public int GetDrinksConsumed()
    {
        return PlayerPrefs.GetInt("drinksConsumed");
    }
}
