using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class b4 : enemy
{

    [SerializeField] float distanceToJump, distanceToHandAttack,jumpTime,rockSpeed;
    //bool up=true;
    float jumpTimeR=0f;
    [SerializeField] GameObject rocks,trees;
    [SerializeField] int NumberOfRocks,RocksDmg,handHitDmg;
    [SerializeField] Transform throwRockPos;
    [SerializeField] float handFlotForce;
    [SerializeField] ParticleSystem crack;
    [SerializeField] LayerMask groundLayer;
    Transform place ;
    [SerializeField] AudioSource jumpSound,startHandAttack;


    private void OnEnable()
    {
        jumpSound.outputAudioMixerGroup = manager.soundsMixer;
        startHandAttack.outputAudioMixerGroup = manager.soundsMixer;
    }
    void handAttack()
    {
        startHandAttack.Play();
        Ray r = new Ray(transform.position, player.position-transform.position);
        RaycastHit hit;
        if(Physics.Raycast(r,out hit,5f,playerLayer))
        {
            attackSound.Play();
            hit.transform.GetComponent<playerStats>().getHealth(handHitDmg);
            hit.transform.GetComponent<playerMVT>().Attack(1f, handFlotForce, player.position - transform.position);
        }

    }
    void runSoundStop()
    {
        walkingSound.Stop();
    }
    void runSOund()
    {
        walkingSound.Play();
    }
    void JumpAttack()
    {
        jumpSound.Play();
        AnimationControle(false, false, false);
        //crack.startRotation3D = Vector3.zero;
        for (int i = 0; i < NumberOfRocks; i++)
        {
            float x = Random.Range(-18f, 0f);
            float y = Random.Range(0, 360f);
            float z = Random.Range(0, 360f);
            GameObject b = Instantiate(rocks, throwRockPos.position, Quaternion.Euler(x, y, z));
            GameObject t = Instantiate(trees, throwRockPos.position, Quaternion.Euler(Random.Range(0f, 360f), y, Random.Range(54f, 114f)));
            t.GetComponent<moveFire>().setStuff(RocksDmg, t.transform.forward, rockSpeed);
            b.GetComponent<moveFire>().setStuff(RocksDmg, b.transform.forward, rockSpeed);
            Destroy(b, 1f);
            Destroy(t, 1f);
        }

        RaycastHit hit;
        if (Physics.Raycast(crack.transform.position + new Vector3(0, 1000, 0), -Vector3.up, out hit, Mathf.Infinity, groundLayer))
        {
            // Debug.Log("dd");
            Transform crackT = crack.transform;
            if(crackT.parent) crackT.SetParent(null); 
            if (!place) place = transform.GetChild(0);
            crackT.position = place.position;
            
            crackT.forward = hit.normal;
            crack.Play();
            
            //crackT.
        }
    }
    void followPlayerB()
    {
        bool isjumping = animator.GetBool("jumpAttack");
        Vector3 playerPos = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
        Vector3 direction = (playerPos - transform.position).normalized;
        transform.forward = direction;
        float distance = Vector3.Distance(playerPos, transform.position);

        if (distance > distanceToJump)
        {
            if (!isjumping)
            {
                runAnimation();
                controller.Move(transform.forward * followSpeed * Time.deltaTime);
            }
            didFindPlayer = false;
        }
        else
        {
            if (!isjumping)
                AnimationControle(false, false, false);
            if (distance > distanceToHandAttack)
            {
                Ray r = new Ray(transform.position, p.position - transform.position);
                if (!Physics.Raycast(r, 20f, blockLayers))
                {
                    if (jumpTimeR <= 0)
                        jump();
                }
            }
            else
            {
                if (!isjumping)
                    handAttackAniamtion();
            }     
        }
        if(jumpTimeR>0)
            jumpTimeR -= Time.deltaTime;
        if (!isjumping)
            controller.Move(transform.up * -30f * Time.deltaTime);
    }
    void AnimationControle(bool jump,bool hand,bool runn)
    {
        animator.SetBool("jumpAttack", jump);
        animator.SetBool("handAttack", hand);
        animator.SetBool("running", runn);
    }
    void runAnimation()
    {
        if(!animator.GetBool("running"))
        AnimationControle(false, false, true);
    }
    void handAttackAniamtion()
    {
        AnimationControle(false, true, false);
    }
    void jump()
    {
        jumpTimeR = jumpTime;
        AnimationControle(true, false, false);
    }
    void Update()
    {
        if (transform.position.y > 37)
        {
            float distance = Vector3.Distance(p.position, transform.position);
            
            if (distance >= 450f)
            {
                if (transform.parent != null)
                    Destroy(transform.parent.gameObject);
                Destroy(gameObject);
            }
            else
            {
                if (player == null)
                {
                    randomMvt();
                    findPlayer();
                }
                else
                {
                    if (distance < 60f)
                        followPlayerB();
                    else
                        player = null;
                }
            }
        }
        else
        {
            goMid();
        }

    }
}
