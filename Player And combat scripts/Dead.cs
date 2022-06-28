using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;


public class Dead : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject deadScreen;
    [SerializeField] private Image image;
    [SerializeField] private Color color1, color2, color3;
    [SerializeField] private AudioSource gunShot;
    [SerializeField] private AudioSource insult;
    [SerializeField] private TextMeshProUGUI[] buttons;
    [SerializeField] private GameObject[] buttonObjects;
    private TextMeshProUGUI textMeshPro;
    private Animator camAnimator;
    private bool playerIsDead;
    private float t;
    void Start()
    {
        camAnimator = GetComponent<Animator>();
        textMeshPro = deadScreen.GetComponentInChildren<TextMeshProUGUI>();
        color1.a = 0f;
        color2.a = 0f;
        t = 0;
        deadScreen.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (playerIsDead && t < 1f )
        {
            t += Time.deltaTime * .1f;
            if(color1.a > .98f)
            {
                color1.a = 1f;
                color2.a = 1f;
            }
            color1.a = Mathf.Lerp(0f, 1f, t);
            color2.a = color1.a;
            color3.a = color1.a;

            buttonObjects.ToList().ForEach(button => button.SetActive(true));
            buttons.ToList().ForEach(buttonText => buttonText.color = color3);
            image.color = color1;
            textMeshPro.color = color2;
            
            if(color1.a > .88f)
            {
                insult.Play();
            }
        }

        
    }

    public void Die()
    {
        playerIsDead = true;
        camAnimator.Play("dead");
        deadScreen.SetActive(true);
        FindObjectOfType<PlayerCombat>().enabled = false;
        gunShot.Play();
        FindObjectOfType<PlayerLook>().enabled = false;
        FindObjectOfType<Player>().enabled = false;
        FindObjectOfType<GameManager>().gameOver = true;
        FindObjectOfType<GameManager>().UnlockCursor();
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(PlayerPrefs.GetInt("Scene"));
    }
}
