using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class soundsMenu : MonoBehaviour
{
    public AudioMixer mixer;
    [SerializeField] Slider vSilder, sSlider;
    float mV, sV;
    [SerializeField] TMP_Dropdown dropDown;
    [SerializeField] RenderPipelineAsset[] levels;
    [SerializeField] Toggle t;
    PlayerCraftAndAttack pCA;

    private void Awake()
    {
        pCA = FindObjectOfType<PlayerCraftAndAttack>();
        
        dropDown.value = 1;
        ChangeGraphic();
    }
    private void Start()
    {
        gameObject.SetActive(false);
    }
    private void OnDisable()
    {
        PlayerPrefs.SetFloat("musicVol", mV);
        PlayerPrefs.SetFloat("soundsVol", sV);
    }
    public void ChangeGraphic()
    {
        QualitySettings.SetQualityLevel(dropDown.value);
        QualitySettings.renderPipeline = levels[dropDown.value];
       // Debug.Log(dropDown.value);
    }
    public void setVolumeMusic(float vol)
     {
        mixer.SetFloat("musicVol", vol);
        mV = vol;
     }
    public void setVolumeSounds(float vol)
    {
        mixer.SetFloat("soundsVol", vol);
        sV = vol;
    }
    public void exite()
    {
        Application.Quit();
    }
    public void setVSslider(float vol,float sound)
    {
        vSilder.value = vol;
        sSlider.value = sound;
    }
    public void setFullScreen()
    {
        Screen.fullScreen = t.isOn;
    }
    public void FixTime()
    {
        pCA.returneTime();
    }

}
