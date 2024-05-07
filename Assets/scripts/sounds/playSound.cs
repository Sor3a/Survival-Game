using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class playSound 
{

    public static void playAudoi(int a,soundManager manager)
    {
        sounds[] sound = manager.sounds[a].sound;
        sound[Random.Range(0, sound.Length)].source.Play();
    }
    public static void stopSound(soundManager manager,int a)
    {
        sounds[] sound = manager.sounds[a].sound;
        sound[0].source.Stop();
    }
}
