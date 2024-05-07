using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mixerManager : MonoBehaviour
{
    public static mixerManager mixer;
    soundsMenu sounds;

    private void Awake()
    {
        if (mixer == null)
        {
            mixer = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
        sounds = FindObjectOfType<soundsMenu>();
        loadSounds();
       // Debug.Log("d");
        loadBinds();
    }

    void loadSounds()
    {
        float musicV = PlayerPrefs.GetFloat("musicVol", 0);
        float soundV = PlayerPrefs.GetFloat("soundsVol", 0);
        sounds.setVolumeMusic(musicV);
        sounds.setVolumeSounds(soundV);
        sounds.setVSslider(musicV, soundV);
    }
    public void loadBinds()
    {
        //Debug.Log(PlayerPrefs.GetString("1", "Z").ToUpper());
        //Debug.Log(PlayerPrefs.GetString("2", "Z").ToUpper());
        KeyCode z = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("1", "Z").ToUpper());
        KeyCode s = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("2", "S").ToUpper());
        KeyCode d = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("3", "D").ToUpper());
        KeyCode q = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("4", "Q").ToUpper());
        KeyCode e = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("5", "E").ToUpper());
        KeyCode i = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("6", "I").ToUpper());
        KeyCode j = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("7", "J").ToUpper());

        panelsControle pControle = FindObjectOfType<panelsControle>();
        if(pControle)
        {
            pControle.setKeys(e, i, j);
        }
        playerMVT mvt = FindObjectOfType<playerMVT>();
        if (mvt)
        {
            mvt.setKeys(z, s, d, q);
            // Debug.Log("kk");
        }

    }

}


