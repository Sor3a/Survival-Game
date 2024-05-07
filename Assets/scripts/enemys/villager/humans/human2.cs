using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class human2 : human
{
    playerStats stats;
    [SerializeField] GameObject FirstSpell,secondSpell;
    [SerializeField] Transform pointOfCast;
    [SerializeField] float speedToAttackSlash,distanceToStartWorking;
    [SerializeField] LayerMask terrainLayer;
    [SerializeField] AudioSource brand;
    void playBrand() { brand.Play(); }
    private void OnEnable()
    {
        stats = FindObjectOfType<playerStats>();
        brand.outputAudioMixerGroup = s.soundsMixer;
    }
    GameObject b;
    void Attack2()
    {
        if(target)
        {
            b = Instantiate(secondSpell, target.position, Quaternion.identity);
            Transform bT = b.transform;
            Ray r = new Ray(target.position + new Vector3(0, 1000f, 0), Vector3.up * -1f);
            RaycastHit hit;
            if (Physics.Raycast(r, out hit, Mathf.Infinity, terrainLayer))
            {
                bT.position = hit.point;
                bT.up = hit.normal;
                bT.Translate(bT.up / 90f);
            }
            bT.GetChild(0).GetComponent<ParticleSystem>().Play();
            Destroy(b, 2f);
        }
    }
    void attack2S(int dmg)
    {
        if(b)
        {
            if (b.GetComponent<spell>().isPlayerIn)
                stats.getHealth(dmg);
        }

    }
    void Attack1(int dmg)
    {

        GameObject b = Instantiate(FirstSpell, pointOfCast.position, Quaternion.identity);
        moveFire move = b.GetComponent<moveFire>();
        Vector3 dir = (target.position - pointOfCast.position).normalized;
        move.setStuff(dmg, dir, speedToAttackSlash);
        Destroy(b, 3f);
    }
    void Attack()
    {

        if (target)
        {
            if (Vector3.Distance(target.position, transform.position) <= distanceToAttack)
            {
                if (walkingSOund.isPlaying)
                    walkingSOund.Stop();

                int a = Random.Range(0, 2);
                if (a > 0)
                {
                    controleAnimation(false, false, true, false);
                }
                else
                {
                    controleAnimation(false, false, false, true);
                }

            }
            else
            {
                controleAnimation(false, true, false, false);
                playMvtSound();
            }
        }
        else
        {
            controleAnimation(true, false, false, false);
            playMvtSound();
        }
           
    }

    void FixedUpdate()
    {
        if (transform.position.y > 37)
        {
            float distance = Vector3.Distance(player.position, transform.position);
            if (!target)
            {
                if (distance < distanceToStartWorking)
                    RandomWalking();
            }
            else
            {
                followTarget();
                if (!animator.GetBool("attack1") && !animator.GetBool("attack2"))
                {
                    Attack();
                }
            }
            if (distance > 600f)
                Destroy(gameObject);
        }
        else
        {
            goMid();
        }

    }
}
