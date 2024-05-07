using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Boss : MonoBehaviour,attackable
{
    Transform player;
    float distance;
    Animator animator;
    CharacterController controller;
    [SerializeField] float lowDistance, mediumDistanc, maxRangeOfAttack, speed, rockSpeed, speedToAttackSlash,legForce;
    [SerializeField] int NumberOfRocks, RocksDmg, legDmg, Health;
    [SerializeField] GameObject rocks, trees, splashSend;
    [SerializeField] Transform throwRockPos;
    [SerializeField] ParticleSystem skyEffect;
    [SerializeField] LayerMask terrainLayer,playerLayer;
    Vector3 direction;
    [SerializeField] Scrollbar sB;
    [SerializeField] AudioSource walking,rocksS,slashS,legSound;
    int initialHealth;
    Rigidbody rb;
    void playSlash()
    {
        slashS.Play();
    }
    void walkS()
    {
        if (!walking.isPlaying)
            walking.Play();
    }
    void stopWalknigS()
    {
        walking.Stop();
    }
    private void OnEnable()
    {
        rb = GetComponent<Rigidbody>();
        skyEffect = FindObjectOfType<skyAttack>().transform.GetComponent<ParticleSystem>();
        initialHealth = Health;
        sB.gameObject.SetActive(true);
        player = FindObjectOfType<playerStats>().transform;
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        sB.size = pourcentage();
    }
    float pourcentage()
    {
        return (float)Health /(float) initialHealth;
    }
    void endGame()
    {
        Cursor.visible = true;
        SceneManager.LoadScene(2);
    }
    public void getHealth(int dmg)
    {
        Debug.Log(dmg);
        Health -= dmg;
        sB.size = pourcentage();
        if (Health<=0)
        {
            endGame();
        }
    }
    void JumpAttack()
    {
        //AnimationControle(false, false, false);
        //crack.startRotation3D = Vector3.zero;
        rocksS.Play();
        for (int i = 0; i < NumberOfRocks; i++)
        {
            float x = Random.Range(-18f, 0f);
            float y = Random.Range(0, 360f);
            float z = Random.Range(0, 360f);
            GameObject b = Instantiate(rocks, throwRockPos.position, Quaternion.Euler(x, y, z));
            GameObject t = Instantiate(trees, throwRockPos.position, Quaternion.Euler(Random.Range(0f, 360f), y, Random.Range(54f, 114f)));
            t.GetComponent<moveFire>().setStuff(RocksDmg, t.transform.forward, rockSpeed);
            b.GetComponent<moveFire>().setStuff(RocksDmg, b.transform.forward, rockSpeed);
            Destroy(b, 1.5f);
            Destroy(t, 1.5f);
        }
    }
    void legAttack()
    {
        Ray r = new Ray(transform.position, player.position - transform.position);
        RaycastHit hit;
        legSound.Play();
        if (Physics.Raycast(r, out hit, 5f, playerLayer))
        {
            hit.transform.GetComponent<playerStats>().getHealth(legDmg);
            hit.transform.GetComponent<playerMVT>().Attack(1f, legForce, player.position - transform.position);
        }
    }
    void sendFire()
    {
        GameObject b = Instantiate(splashSend, throwRockPos.position, Quaternion.identity);
        moveFire move = b.GetComponent<moveFire>();
        b.GetComponent<ParticleSystem>().Play();
        Vector3 dir = (player.position - throwRockPos.position).normalized;
        move.setStuff(35, dir, speedToAttackSlash);
        Destroy(b, 4f);
    }
    IEnumerator createEffect(Vector3 pos)
    {
        yield return new WaitForSeconds(.5f);
        Ray r = new Ray(pos + new Vector3(0, 1000, 0), -1f * Vector3.up);
        RaycastHit hit;
        if(Physics.Raycast(r,out hit,Mathf.Infinity, terrainLayer))
        {
            skyEffect.transform.position = hit.point;
            skyEffect.Play();
            skyEffect.gameObject.GetComponent<skyAttack>().lightS();
        }    
    }
    void skyAttack()
    {
        Vector3 playerPos = player.position;
        StartCoroutine(createEffect(playerPos));
    }
    void setBool(string animation, bool value)
    {
        animator.SetBool(animation, value);
    }
    void endLegAttack()
    {
        if (distance <= lowDistance)
        {
            setBool("lowDistance", true);
            return;
        }
        else
        {
            setBool("lowDistance", false);
            int a;
            a = Random.Range(0, 3);
            if (a == 0) run();
            else if (a == 1) setBool("medDistance", true);
            else
            {
                setBool("heighDistance", true);
                animator.SetInteger("attack", 1);
            }

        }
    }
    void endRunAttack()
    {
        setBool("medDistance", false);
        if (distance <= lowDistance)
            setBool("lowDistance", true);
        else if (distance > lowDistance && distance <= mediumDistanc - 5f)
        {
            int a = Random.Range(0, 2);
            if (a == 0)
                setBool("medDistance", true);
            else
                idle();
        }
        else //if (distance > mediumDistanc-1)
        {
            int a = Random.Range(0, 2);
            if (a == 0)
                run();
            else
            {
                setBool("medDistance", false);
                setBool("heighDistance", true);
                animator.SetInteger("attack", 1);

            }
        }


    }
    void endWalk()
    {
        setBool("walking", false);
        if (distance <= lowDistance)
        {
            setBool("lowDistance", true);
            setBool("medDistance", false);

        }
        else if (distance > lowDistance && distance <= maxRangeOfAttack)
        {
            if (Random.Range(0, 3) == 0)
            {
                setBool("heighDistance", false);
                setBool("medDistance", true);
                setBool("lowDistance", false);
            }
            else
            {
                setBool("medDistance", false);
                setBool("heighDistance", true);
                animator.SetInteger("attack", Random.Range(0, 2));
            }
        }
        else
            run();
    }
    void finishIdle()
    {
        animator.SetBool("idle", false);
        if (distance > lowDistance)
        {
            if (Random.Range(0, 2) > 0)
            {
                if (Random.Range(0, 3) == 0)
                    run();
                else
                {
                    animator.SetInteger("attack", Random.Range(0, 2));
                    setBool("medDistance", false);
                    setBool("heighDistance", true);
                }
            }
            else
                midAttack();
        }
        else
        {
            setBool("lowDistance", true);
            setBool("medDistance", false);
            setBool("heighDistance", false);
        }

    }
    void idle()
    {
        setBool("lowDistance", false);
        setBool("medDistance", false);
        setBool("heighDistance", false);
        setBool("idle", true);
    }
    void midAttack()
    {
        animator.SetBool("lowDistance", false);
        animator.SetBool("medDistance", true);
        animator.SetBool("heighDistance", false);
    }

    void changeAttack()
    {
        setBool("medDistance", false);
        if (distance > mediumDistanc && distance < maxRangeOfAttack)
        {
            bool a = Random.Range(0, 3) == 0 ? true : false;
            setBool("walking", a);
            setBool("heighDistance", !a);
            animator.SetInteger("attack", Random.Range(0, 2));
        }
        else if (distance > maxRangeOfAttack)
        {
            setBool("medDistance", false);
            setBool("heighDistance", false);
            run();
        }
        else if (distance < lowDistance)
        {
            setBool("medDistance", false);
            setBool("heighDistance", false);
            setBool("lowDistance", true);
        }
        else
            idle();

    }
    //void longDistanceAttack()
    //{
    //    int k = Random.Range(0, 3)>0?1:0;
    //    if(k==0)
    //    {
    //        midAttack();
    //    }
    //    else
    //    {
    //        animator.SetInteger("attack", Random.Range(0, 2));
    //        animator.SetBool("heighDistance", true);
    //    }
    //}
    //void legAttack()
    //{
    //        animator.SetBool("lowDistance", true);
    //        animator.SetBool("medDistance", false);

    //}
    void run()
    {
        animator.SetBool("walking", true);
        animator.SetBool("lowDistance", false);
        animator.SetBool("medDistance", false);
        animator.SetBool("heighDistance", false);
    }
    void FixRotation()
    {
        float angle = Vector3.SignedAngle(Vector3.forward, direction, Vector3.up);
        controller.Move(Vector3.up * -8f * Time.deltaTime);
       // rb.MoveRotation(Quaternion.Euler(0, -angle, 0));
        transform.rotation = Quaternion.Euler(0, angle, 0);
    }
    void Update()
    {
        direction = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
        Vector3 d = (new Vector3(player.position.x, transform.position.y, player.position.z)- transform.position ).normalized ;
        distance = Vector3.Distance(player.position, transform.position);
        //if (!animator.GetBool("idle"))
        FixRotation();
        if (animator.GetBool("walking") && distance > 4f)
        {
            //rb.MovePosition(transform.position + d* Time.deltaTime * speed);
            controller.Move(transform.forward * Time.deltaTime * speed);
        }
           
    }
}
