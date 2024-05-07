using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skeleton : enemy
{
    [SerializeField] float findRadSkeleton;
    [SerializeField] int dmg;
    int initialHealthe;
    bool canStart;
    Transform Eplayer;
    private void Start()
    {
        initialHealthe = heath;
        Eplayer = FindObjectOfType<playerMVT>().transform;
    }
    void attackPlayer()
    {
        stats.getHealth(dmg);
        speedToAttackR = speedToAttack;
    }
    bool isIdle = false;
    void findPlayere()
    {
        if(!isIdle)
        {
            setAnimationIdle();
            isIdle = true;
        }
        
        if (Vector3.Distance(transform.position, Eplayer.position) < findRadSkeleton)
        {
            player = Eplayer;
            stats = player.GetComponent<playerStats>();
            animator.SetBool("startMoving", true);
            StartCoroutine(startWalking());
        }
    }
    IEnumerator startWalking()
    {
        yield return new WaitForSeconds(3f);
        animator.SetBool("startMoving", false);
        canStart = true;
        //heath = initialHealthe;
    }
    bool abb;
    void Update()
    {
        if (player == null)
        {
            //Debug.Log(initialHealthe);
            findPlayere();
            //if(heath!= initialHealthe)
            heath = initialHealthe;
        }  
        else if(player !=null && !canStart)
        {
            if(!abb)
            {
                heath = initialHealthe;

                stats = player.GetComponent<playerStats>();
                animator.SetBool("startMoving", true);
                StartCoroutine(startWalking());
                abb = true;
            }

        }
        else if(canStart)
        {
           // Debug.Log("gd");
            //stats = player.GetComponent<playerStats>();
            //animator.SetBool("startMoving", true);
            //StartCoroutine(startWalking());
            followPlayer();
        }
            
    }
}
