using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class MoneyUI : MonoBehaviour
{
    private TextMeshProUGUI text;

    public bool updateMoney;
    private DialogueManager dialogueManager;
    
    private void Awake()
    {
        dialogueManager = FindObjectOfType<DialogueManager>();
        text = GetComponent<TextMeshProUGUI>();
        text.text = PlayerPrefs.GetInt("money").ToString();
    }

    // Update is called once per frame
    private void Update()
    {
        text.text = dialogueManager.money.ToString();
    }
}
