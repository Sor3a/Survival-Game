using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arrowSound : MonoBehaviour
{
    [SerializeField] AudioSource arrowImpact; 
    public void CreateSound(Vector3 pos)
    {
        GameObject b = new GameObject();
        b.transform.position = pos;
        AudioSource a = b.AddComponent<AudioSource>();
        //a = arrowImpact;
        a.clip = arrowImpact.clip;
        a.volume = arrowImpact.volume;
        a.spatialBlend = arrowImpact.spatialBlend;
        a.rolloffMode = AudioRolloffMode.Linear;
        a.maxDistance = 40;
        a.Play();
        Destroy(b, 3f);


    }
}
