using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class b5 : enemy
{
    [SerializeField] float distanceToThrowRock, distanceToJump, timeToRock, rockSpeed, rocksSpeed, FlotForce;//rockSpeed : speed of 1 rock other so many rocks
    float timeToRockR = 0;
    [SerializeField] int rockDmg, NumberOfRocks, jumpAttackDmg, jumpDmg;
    [SerializeField] GameObject rock, rockPrefab, trees;
    [SerializeField] Transform throwRockPos;
    bool canattack = true;
    [SerializeField] AudioSource rockSource,jumpSound;

    private void OnEnable()
    {
        jumpSound.outputAudioMixerGroup = manager.soundsMixer;
        rockSource.outputAudioMixerGroup = manager.soundsMixer;
    }
    void setRockActive()
    {
        rock.SetActive(true);
    }
    void jumpImpact()
    {
        jumpSound.Play();
    }
    void throwR()
    {
        if (player)
        {
            Transform rockT = rock.transform;
            GameObject r = Instantiate(rockPrefab, rockT.position, rockT.rotation);
            r.transform.localScale = new Vector3(6, 6, 6);
            moveFire rockMVT = r.GetComponent<moveFire>();
            rockMVT.setStuff(rockDmg, (player.position - r.transform.position).normalized, rockSpeed);
            rockMVT.isRock = true;
            AnimationControle(false, false, false);
            rock.SetActive(false);
            Destroy(r, 3f);
        }
        AnimationControle(false, false, false);
        rock.SetActive(false);

    }//throw rock function
    void followPlayerB()
    {
        bool isThrowing = animator.GetBool("throwRock");
        Vector3 playerPos = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
        transform.forward = (playerPos - transform.position).normalized;
        float distance = Vector3.Distance(playerPos, transform.position);
        if (distance > distanceToThrowRock)
        {
            if (!isThrowing)
            {
                if (!animator.GetBool("running"))
                    runAnimation();
                Ray r = new Ray(transform.position, p.position - transform.position);
                if (!Physics.Raycast(r, 20f, blockLayers))
                {
                    controller.Move(transform.forward * followSpeed * Time.deltaTime);
                }
                else
                    player = null;

            }
            didFindPlayer = false;
        }
        else
        {
            if (!isThrowing)
                AnimationControle(false, false, false);
            if (distance > distanceToJump)
            {
                if (timeToRockR <= 0 && player)
                    rockAnimation();
            }
            else
            {
                if (!isThrowing && canattack)
                {
                    jumpAttack();
                }

            }
        }
        if (timeToRockR > 0)
            timeToRockR -= Time.deltaTime;
        if (!isThrowing && !animator.GetBool("jumpAttack"))
        {
            controller.Move(transform.up * -30f * Time.deltaTime);
        }

    }
    void runStop()
    {
        walkingSound.Stop();
    }
    void jumpAttack()
    {
        AnimationControle(true, false, false);
       
    }
    void canattackAgain()
    {
        canattack = true;
    }
    void CreateRocksAndSuff()
    {
        jumpImpact();
        canattack = false;
        AnimationControle(false, false, false);
        for (int i = 0; i < NumberOfRocks; i++)
        {
            float x = Random.Range(-18f, 0f);
            float y = Random.Range(0, 360f);
            float z = Random.Range(0, 360f);
            GameObject b = Instantiate(rockPrefab, throwRockPos.position, Quaternion.Euler(x, y, z));
            GameObject t = Instantiate(trees, throwRockPos.position, Quaternion.Euler(Random.Range(0f, 360f), y, Random.Range(54f, 114f)));
            t.GetComponent<moveFire>().setStuff(jumpAttackDmg, t.transform.forward, rocksSpeed);
            b.GetComponent<moveFire>().setStuff(jumpAttackDmg, b.transform.forward, rocksSpeed);
            Destroy(b, 1f);
            Destroy(t, 1f);
        }
        Ray r = new Ray(transform.position, player.position - transform.position);
        RaycastHit hit;
        if (Physics.Raycast(r, out hit, 5f, playerLayer))
        {

            hit.transform.GetComponent<playerStats>().getHealth(jumpDmg);

            hit.transform.GetComponent<playerMVT>().Attack(1f, FlotForce, player.position - transform.position);
        }
    }
    void getRockSound()
    {
        rockSource.Play();
    }
    void AnimationControle(bool jump, bool rock, bool runn)
    {
        animator.SetBool("jumpAttack", jump);
        animator.SetBool("throwRock", rock);
        animator.SetBool("running", runn);
        
    }
    void rockAnimation()
    {
        timeToRockR = timeToRock;
        AnimationControle(false, true, false);
    }
    void runSOund()
    {
        if(!walkingSound.isPlaying)
        walkingSound.Play();
    }
    void runAnimation()
    {
        if (!animator.GetBool("running"))
            AnimationControle(false, false, true);
        
    }
    void Update()
    {
        if (transform.position.y > 37)
        {
            float distance = Vector3.Distance(p.position, transform.position);
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
            if (distance >= 450f)
            {
                if (transform.parent != null)
                    Destroy(transform.parent.gameObject);
                Destroy(gameObject);
            }
        }
        else
        {
            goMid();
        }
            
    }
}
