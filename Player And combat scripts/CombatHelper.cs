using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatHelper : MonoBehaviour
{

    [SerializeField] private AudioSource[] punchAudio;
    [SerializeField] private AudioSource missAudio;
    
    public void PlayPunch()
    {
        // plays random punch sound effect
        punchAudio[UnityEngine.Random.Range(0, punchAudio.Length)].Play();
    }

    public void PlayMiss()
    {
        missAudio.Play();
    }
}
