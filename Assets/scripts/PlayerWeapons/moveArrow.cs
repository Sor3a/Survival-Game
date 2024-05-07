using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveArrow : MonoBehaviour
{

    float speed, x, y, z = 0f;
    Rigidbody rb;
    playerStats stats;
    ParticleSystem p, p2;
    public void setStuff(float s, ParticleSystem pa, ParticleSystem p2)
    {
        p = pa;
        this.p2 = p2;
        speed = s;
        rb = transform.GetComponent<Rigidbody>();
        stats = FindObjectOfType<playerStats>();
        rb.AddForce(-transform.forward * speed, ForceMode.Impulse);
        x = transform.rotation.eulerAngles.x;
        y = transform.rotation.eulerAngles.y;
        z = transform.rotation.eulerAngles.z;
    }
    private void Update()
    {
        if (z != 0f)
            transform.rotation = Quaternion.Euler(x -= 0.05f, y, z);
    }
    private void OnTriggerEnter(Collider other)
    {
        GameObject hit = other.gameObject;
        if (hit.layer == 9 || hit.layer == 16)
        {
            arrowSound s = GetComponent<arrowSound>();
            if (s)
            {
                s.CreateSound(transform.position);
            }

            attackable target = hit.GetComponent<attackable>();
            if (target != null) target.getHealth(stats.damage);

            Animal a = hit.GetComponent<Animal>();
            if (a)
            {
                p2.transform.position = transform.position;
                p2.Play();
            }
            else
            {
                p.transform.position = transform.position;
                p.Play();
            }

        }
        Destroy(transform.parent.gameObject);
    }

}
