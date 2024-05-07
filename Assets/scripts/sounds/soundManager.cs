using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class soundManager : MonoBehaviour
{
    [System.Serializable] 
    public class ss
    {
       public sounds[] sound;
       public string name;
    }
    public ss[] sounds;
    public AudioMixerGroup soundsMixer;
    
    private void Awake()
    {
        foreach (ss item in sounds)
        {
            foreach (sounds sound in item.sound)
            {
                sound.source = gameObject.AddComponent<AudioSource>();
                sound.source.clip = sound.Audio;
                sound.source.volume = sound.volume;
                sound.source.playOnAwake = false;
                sound.source.loop = sound.repeat;
                sound.source.outputAudioMixerGroup = soundsMixer;
            }
        }
    }

}
