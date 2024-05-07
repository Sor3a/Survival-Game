using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class backGroundMusic : MonoBehaviour
{
    [SerializeField] AudioSource s;
    AudioSource[] audios;
    AudioSource clipPlaying;
    DayAndNight d;
    AudioSource endAudio;
    private void Awake()
    {
        audios = GetComponents<AudioSource>();
        d = FindObjectOfType<DayAndNight>();
        endAudio = transform.GetChild(0).GetComponent<AudioSource>();
    }

    private void Start()
    {
        s.Play();
        clipPlaying = s;
    }
    int i=0;
    private void Update()
    {
        if(!d.ended)
        {
            if (!clipPlaying.isPlaying)
            {
                 i = Random.Range(0, audios.Length);
                clipPlaying = audios[i];
                audios[i].Play();
            }
        }
        else
        {
            if(!endAudio.isPlaying)
            {
                clipPlaying.Stop();
                endAudio.Play();
            }
            
            
        }

    }
}
