using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveFire : MonoBehaviour
{
    int damag;
    Vector3 direction;
    float fireSpeed;
    [HideInInspector] public bool isRock = false;
    float z, x, y;
    [SerializeField] GameObject effect;
    [SerializeField] LayerMask layer;
    public void setStuff(int dmg, Vector3 dir, float FireSpeed)
    {
        damag = dmg;
        direction = dir;
        fireSpeed = FireSpeed;
        speed = Random.Range(fireSpeed - 5f, fireSpeed + 5f);
    }

    private void OnTriggerEnter(Collider other)
    {

        if(effect)
        {
            GameObject b = Instantiate(effect, transform.position, Quaternion.identity);
            RaycastHit hit;
            if(Physics.Raycast(transform.position+ new Vector3(0,10f,0),-Vector3.up,out hit,30f,layer))
            {
                b.transform.position = hit.point;
                b.transform.up = hit.normal;
            }
            b.GetComponent<ParticleSystem>().Play();
            b.GetComponent<AudioSource>().Play();
            Destroy(b, 4f);
        }    
        if (other.gameObject.layer == 7)
        {
            playerStats a = other.gameObject.GetComponent<playerStats>();
            if (a)
                a.getHealth(damag);
        }
        Destroy(gameObject);
    }
    float speed;
    private void FixedUpdate()
    {
        if (direction != null)
        {
             
            if (direction != Vector3.zero)
                transform.forward = direction;
            transform.Translate(direction * speed * Time.deltaTime, Space.World);
        }
        if (isRock)
            transform.rotation = Quaternion.Euler(x++, y++, z++);
    }
}
