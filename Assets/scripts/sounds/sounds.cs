using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


[System.Serializable]
public class sounds 
{
    public AudioClip Audio;
    [Range(0f,1f)]
    public float volume = 0.8f;
    public bool repeat = false;
   [HideInInspector] public AudioSource source;
}
