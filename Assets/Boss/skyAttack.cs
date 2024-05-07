using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skyAttack : MonoBehaviour
{
    ParticleSystem p;
    [SerializeField] int startingDmg, stayingDmg;
    [SerializeField] AudioSource lighingSound;
    public void lightS()
    {
        lighingSound.Play();
    }

    private void OnEnable()
    {
        p = GetComponent<ParticleSystem>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(p.isPlaying && other.tag == "Player")
        {
            other.gameObject.GetComponent<playerStats>().getHealth(startingDmg);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (p.isPlaying && other.tag == "Player")
        {
            other.gameObject.GetComponent<playerStats>().getHealth(stayingDmg);
        }
    }
}
