using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class human1 : human
{
    [SerializeField] GameObject splashSend;
    [SerializeField] Transform pointOfSlash;
    [SerializeField] float speedToAttackSlash, distanceToStartWorking;
    [SerializeField] LayerMask playerLayer;
    playerStats stats;
    [SerializeField] AudioSource handAttack;
    void playHand() { handAttack.Play(); }
    private void OnEnable()
    {
        stats = FindObjectOfType<playerStats>();
        handAttack.outputAudioMixerGroup = s.soundsMixer;
    }
    void attack1(int dmg)
    {
        GameObject b = Instantiate(splashSend, pointOfSlash.position, Quaternion.identity);
        moveFire move = b.GetComponent<moveFire>();
        Vector3 dir = (target.position - pointOfSlash.position).normalized;
        move.setStuff(dmg, dir, speedToAttackSlash);
        Destroy(b, 3f);
    }
    void attack2(int dmg)
    {
        playHand();
        Ray r = new Ray(transform.position, (target.position - transform.position).normalized);
        RaycastHit hit;
        if (Physics.Raycast(r, out hit, 4f, playerLayer))
        {
            stats.getHealth(dmg);
        }
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
                {
                    RandomWalking();
                }
                    
                

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
            goMid();



    }
}
