using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public bool newGame;
    private PlayerPrefSaving playerPref;
    [SerializeField] private GameObject warningMenu;
    // Start is called before the first frame update

    private void Awake()
    {
        playerPref = FindObjectOfType<PlayerPrefSaving>();
    }

    public void OnNewGameClicked()
    {
        
        newGame = true;
        PlayerPrefs.DeleteAll();
        playerPref.SetPlayerMoney(25);
        playerPref.SetPlayerHealth(400);
        playerPref.SetMinFOV(100);
        playerPref.SetMaxFov(100);
        
        playerPref.SetPunchDamage(10);
        playerPref.SetKickDamage(10);

        PlayerPrefs.Save();
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
    }


    public void OnContinueGameClicked()
    {
        if(playerPref.GetPrefsScene() > 0)
            SceneManager.LoadScene(PlayerPrefs.GetInt("Scene"));
        else
        {
            warningMenu.SetActive(true);
        }
    }

    public void OnQuitPressed()
    {
        Application.Quit();
    }
    public void ExitWarningOptionPressed()
    {
        warningMenu.SetActive(false);
        
    }
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

   
}
