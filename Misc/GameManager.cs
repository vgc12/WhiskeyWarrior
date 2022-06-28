using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public bool gameOver;
    private PlayerPrefSaving prefSaving;

    

    void Start()
    {
        LockCursor();
        gameOver = false;
        //DontDestroyOnLoad(gameObject);
        prefSaving = FindObjectOfType<PlayerPrefSaving>();
    }

    

    // Update is called once per frame
    void Update()
    {
        if (!DialogueVariables.dialogueIsPlaying && !gameOver && SceneManager.GetActiveScene().buildIndex != 0)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                UnlockCursor();
            }

            if (Input.GetMouseButton(0) && !gameOver)
            {
                LockCursor();
            }
        }
        else
        {
            UnlockCursor();
        }
        

       
       
    }



    private void OnLevelWasLoaded()
    {
        if (SceneManager.GetActiveScene().buildIndex != 0)
            prefSaving.SetPrefsScene(SceneManager.GetActiveScene().buildIndex);
    }


    public void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void LoadScene()
    {
        SceneManager.LoadScene(PlayerPrefs.GetInt("Scene"));
    }

    public void LoadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        
    }

    public void Restart()
    {
        Debug.Log("button Clicked");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        gameOver = false;
    }
}
