using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class slowBomb : enemy
{
    [SerializeField] int dmg ;
    [SerializeField] GameObject bombEffect;
    void bomb()
    {
        GameObject b=  Instantiate(bombEffect,transform.position,Quaternion.identity);
        b.GetComponent<ParticleSystem>().Play();
        Destroy(b, 3f);
        stats.getHealth(dmg);
        Destroy(gameObject);
    }




}
